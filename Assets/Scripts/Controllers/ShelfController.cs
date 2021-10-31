using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Valve.VR.InteractionSystem;
using Debug = Sisus.Debugging.Debug;

public class ShelfController : MonoBehaviour {

    public enum ShelfMode {
        Category,
        Search
    }

    public ShelfMode mode = ShelfMode.Category;

    public List<string> categories;
    public bool displayAll = false;
    public GameObject itemPrefab;
    public GameObject slotPrefab;
    [Tooltip("Object to parent the slots to")]
    public GameObject categorySlot;
    public ProductsController productsController;
    private int _perPage = 3;
    public int spacing = 0;

    public TMPro.TextMeshProUGUI categoryNameDisplay;

    public delegate void CategoryPage(int previous, int next);
    public CategoryPage OnPageChanged;

    private List<Product> _finalProducts;
    private int totalProductCount = 0;

    private int _page = 1;

    [field: SerializeField]
    public int Page
    {
        get {
            return _page;
        }
        set {

            int upperLimit = 1;

            if (value <= 0) {
                value = 1;
            }
            else if (_finalProducts != null) {
                upperLimit = Mathf.CeilToInt((float)_finalProducts.Count / _perPage);
                if (value > upperLimit ) {
                    value = upperLimit;
                }
            }
            if(_page != value) {
                OnPageChanged?.Invoke(_page, value);
                _page = value;
            }
        }
    }


    private float categoryWidth = 0.0f;
    private float itemPrefabWidth = 0.0f;
    // Start is called before the first frame update
    void Start() {
        productsController.OnProductsLoaded += OnProductsLoaded;
        OnPageChanged += PageChanged;

        categoryWidth = categorySlot.GetComponent<BoxCollider>().size.x * categorySlot.transform.lossyScale.x;
        itemPrefabWidth = itemPrefab.GetComponent<BoxCollider>().size.x * itemPrefab.transform.lossyScale.x;
        _perPage = Mathf.FloorToInt(categoryWidth / itemPrefabWidth);

        categoryNameDisplay.text = "";

        foreach(string category in categories) {
            categoryNameDisplay.text += category + " | ";
        }
        categoryNameDisplay.text.Trim(' ');
        categoryNameDisplay.text.Trim('|');
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator DisplayProducts(int page) {
        if(_finalProducts == null) {
            yield break;
        }

        ClearChildren();

        if (Page < 0) {
            yield break;
        }

        float slotOffset = (categoryWidth / _perPage) / 2;
        int indexOffset = _perPage * (page - 1);

        for (int i = 0; i < _perPage; ++i) {

            if((i + indexOffset + 1) > _finalProducts.Count) {
                break;
            }

            Product product = _finalProducts[i + indexOffset];

            GameObject slot = Instantiate(slotPrefab, categorySlot.transform);

            slot.GetComponent<BoxCollider>().size = itemPrefab.GetComponent<BoxCollider>().size;

            //Calculate position of each slot
            float finalSpacing = (i == 0 ? 0.0f : spacing);

            slot.transform.localPosition = new Vector3(slot.transform.localPosition.x + (categoryWidth / 2) - slotOffset - (slotOffset * 2 *i) - finalSpacing, slot.transform.localPosition.y, slot.transform.localPosition.z);

            GameObject item = Instantiate(itemPrefab, slot.transform);

            ItemPropertiesScript properties = item.GetComponent<ItemPropertiesScript>();

            properties.SetProduct(product);

            //Download the respective thumbnails

            if (product.Thumbnail == null) {
                productsController.GetProductImage(product.productInfo.images[0].src, result => {
                    Debug.Log("[Download Manager][Complete] %s", product.productInfo.images[0].src);
                    product.Thumbnail = Sprite.Create(result, new Rect(0.0f, 0.0f, result.width, result.height), new Vector2(0.5f, 0.5f));
                });
            }

            //Match the model prefabs to the products

            GameObject obj = Resources.Load("Prefabs/Model Prefabs/" + product.productInfo.sku) as GameObject;

            if(obj == null) {
                continue;
            }

            GameObject instance = Instantiate(obj, properties.modelDisplaySlot.transform);

            ModelProperties modelProperties = instance.GetComponent<ModelProperties>();
            if (modelProperties) {
                if (modelProperties.SKU == product.productInfo.sku) {
                    properties.GetProduct().modelPrefab = obj;

                    Collider collider = instance.GetComponent<Collider>();
                    Collider displayCollider = properties.modelDisplaySlot.GetComponent<Collider>();

                    foreach (Collider col in instance.GetComponentsInChildren<Collider>()) {
                        col.enabled = false;
                    }

                        
                    Destroy(instance.GetComponent<InteractionsController>());
                    Destroy(instance.GetComponent<VelocityEstimator>());
                    Destroy(instance.GetComponent<ComplexThrowable>());
                    Destroy(instance.GetComponent<Throwable>());
                    Destroy(instance.GetComponent<Interactable>());
                    Destroy(instance.GetComponent<Rigidbody>());

                    //We need those two things active to get the bounds. We can deactivate them after
                    properties.modelDisplaySlot.SetActive(true);
                    displayCollider.enabled = true;
                    collider.enabled = true;

                    //Rescale to fit in the display

                    if (collider.bounds.size.x > displayCollider.bounds.size.x) {
                        float scale = displayCollider.bounds.size.x / collider.bounds.size.x;

                        instance.transform.localScale *= scale;
                    }

                    Physics.SyncTransforms();

                    if (collider.bounds.size.y > displayCollider.bounds.size.y) {
                        float scale = displayCollider.bounds.size.y / collider.bounds.size.y;

                        instance.transform.localScale *= scale;
                    }

                    Physics.SyncTransforms();

                    if (collider.bounds.size.z > displayCollider.bounds.size.z) {
                        float scale = displayCollider.bounds.size.z / collider.bounds.size.z;

                        instance.transform.localScale *= scale;
                    }

                    Helpers.SnapToBoundInternal(Helpers.Sides.BOTTOM, instance, properties.modelDisplaySlot);

                    properties.modelDisplaySlot.SetActive(false);
                    displayCollider.enabled = false;
                    collider.enabled = false;
                    yield return null;
                }
            }
            else {
                Destroy(instance);
                Resources.UnloadAsset(obj);
            }
            yield return null;
        }
        yield break;
    }

    public void OnProductsLoaded() {
        if (productsController.List != null) {
            //If we want to display everything on one shelf here's an option :)

            if (displayAll && mode != ShelfMode.Search) {
                _finalProducts = productsController.List;
            }
            else if (mode == ShelfMode.Category) {
                if(categories.Count <= 0) {
                    categories.Add("Uncategorized");
                }
                _finalProducts = new List<Product>();

                foreach (Product product in productsController.List) {
                    foreach (ProductCategory productCategory in product.productInfo.categories) {
                        foreach (string targetCategory in categories) {

                            //Compare case-insensitive

                            if (productCategory.name.Contains(targetCategory, System.StringComparison.CurrentCultureIgnoreCase)) {
                                _finalProducts.Add(product);
                            }
                        }
                    }
                }
            }
            else {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(DisplayProducts(Page));
        }
    }

    public void ChangePage(int pageChange) {
        Page += pageChange;
    }

    public void PageChanged(int previous, int next) {
        StopAllCoroutines();
        StartCoroutine(DisplayProducts(next));
    }

    public void ClearChildren() {
        foreach (Transform child in categorySlot.transform) {
            Destroy(child.gameObject);
        }
    }
    public void LoadProducts(List<Product> productList) {
        _finalProducts = productList;
        Page = 1;
        StopAllCoroutines();
        StartCoroutine(DisplayProducts(Page));
    }
}
