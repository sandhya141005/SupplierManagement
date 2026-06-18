let isEdit = false;
let supplierId = 0;
let deletedProductIds = [];

function initializeSupplierForm(options) {
    isEdit = options.isEdit;
    supplierId = options.supplierId;
    deletedProductIds = [];

    function showError(messages) {
        const box = document.getElementById('jsErrorBox');
        if (!Array.isArray(messages)) messages = [messages];
        box.innerHTML = messages.map(m => `<div>${m}</div>`).join('');
        box.style.display = 'block';
        box.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    function clearError() {
        const box = document.getElementById('jsErrorBox');
        box.innerHTML = '';
        box.style.display = 'none';
    }

    function getProductCardCount() {
        return document.querySelectorAll('#productsContainer .product-card').length;
    }

    function updateProductCountLabel() {
        const total = parseInt(document.getElementById("TotalProducts").value, 10);
        const label = isNaN(total) ? '?' : total;
        document.getElementById("productCountLabel").textContent =
            `(${getProductCardCount()} / ${label} added)`;
    }

    document.getElementById('CountryId').addEventListener('change', function () {
        const countryId = this.value;
        const stateDropdown = document.getElementById('StateId');
        const cityDropdown = document.getElementById('CityId');
        stateDropdown.innerHTML = '<option value="">Select state</option>';
        cityDropdown.innerHTML = '<option value="">Select city</option>';
        if (!countryId) return;
        fetch(`/Auth/GetStates?countryId=${countryId}`)
            .then(r => r.json())
            .then(states => {
                states.forEach(s => {
                    const opt = document.createElement('option');
                    opt.value = s.value;
                    opt.text = s.text;
                    stateDropdown.appendChild(opt);
                });
            });
    });

    document.getElementById('StateId').addEventListener('change', function () {
        const stateId = this.value;
        const cityDropdown = document.getElementById('CityId');
        cityDropdown.innerHTML = '<option value="">Select city</option>';
        if (!stateId) return;
        fetch(`/Auth/GetCities?stateId=${stateId}`)
            .then(r => r.json())
            .then(cities => {
                cities.forEach(c => {
                    const opt = document.createElement('option');
                    opt.value = c.value;
                    opt.text = c.text;
                    cityDropdown.appendChild(opt);
                });
            });
    });

    document.getElementById("addProductBtn").addEventListener("click", function () {
        clearError();
        const rawValue = document.getElementById("TotalProducts").value;
        const totalProducts = parseInt(rawValue, 10);
        if (rawValue === "" || isNaN(totalProducts) || totalProducts <= 0) {
            showError("Please enter the number of products first");
            return;
        }
        if (getProductCardCount() >= totalProducts) {
            showError("Maximum products reached");
            return;
        }
        const category = document.getElementById("CatalogType").value;
        const html = `
            <div class="product-card" data-product-id="0"
                 style="border:1px solid #ddd;border-radius:8px;padding:12px;margin-bottom:10px;position:relative;">
                <button type="button" class="btn-danger-rc btn-sm removeProductBtn"
                        style="position:absolute;top:8px;right:8px;">&times;</button>
                <input type="hidden" class="prod-id" value="0" />
                <div style="display:grid;grid-template-columns:1fr 1fr 1fr 1fr;gap:0 1rem;">
                    <div class="form-group">
                        <label>Product Name</label>
                        <input class="prod-name" placeholder="Product Name" />
                    </div>
                    <div class="form-group">
                        <label>Price</label>
                        <input class="prod-price" placeholder="Price" type="number" step="0.01" />
                    </div>
                    <div class="form-group">
                        <label>Discount</label>
                        <input class="prod-discount" placeholder="Discount" type="number" step="0.01" value="0" />
                    </div>
                    <div class="form-group">
                        <label>Available Stock</label>
                        <input class="prod-stock" placeholder="Stock" type="number" />
                    </div>
                    <div class="form-group" style="grid-column:1/-1;">
                        <label>Category</label>
                        <input class="prod-category" value="${category}" readonly />
                    </div>
                </div>
            </div>`;
        document.getElementById("productsContainer").insertAdjacentHTML("beforeend", html);
        updateProductCountLabel();
    });

    document.getElementById("productsContainer").addEventListener("click", function (e) {
        if (e.target.classList.contains("removeProductBtn")) {
            const card = e.target.closest(".product-card");
            if (isEdit) {
                const productId = parseInt(card.querySelector(".prod-id").value, 10);
                if (productId > 0) deletedProductIds.push(productId);
            }
            card.remove();
            updateProductCountLabel();
        }
    });

    document.getElementById("TotalProducts").addEventListener("input", updateProductCountLabel);
 function clearInlineErrors() {
        document.querySelectorAll('.prod-field-error').forEach(el => el.remove());
        document.querySelectorAll('.input-error').forEach(el => el.classList.remove('input-error'));
    }

    function markFieldError(input, message) {
        input.classList.add('input-error');
        const existing = input.parentElement.querySelector('.prod-field-error');
        if (existing) existing.remove();
        const span = document.createElement('span');
        span.className = 'prod-field-error';
        span.textContent = message;
        input.insertAdjacentElement('afterend', span);
    }
    document.getElementById("supplierForm").addEventListener("submit", function (e) {
        clearError();
        clearInlineErrors();

        const total = parseInt(document.getElementById("TotalProducts").value, 10);
        const cards = document.querySelectorAll('#productsContainer .product-card');

        if (isNaN(total) || total <= 0) {
            e.preventDefault();
            showError("Please enter the number of products");
            return;
        }
        if (cards.length === 0) {
            e.preventDefault();
            showError("At least one product is required");
            return;
        }
        if (cards.length !== total) {
            e.preventDefault();
            showError(cards.length < total
                ? `Add ${total - cards.length} more products or update Total Products`
                : `Remove ${cards.length - total} products or update Total Products`);
            return;
        }

        let hasError = false;

        cards.forEach((card) => {
            const nameInput= card.querySelector(".prod-name");
            const priceInput = card.querySelector(".prod-price");
            const stockInput= card.querySelector(".prod-stock");
            const discountInput= card.querySelector(".prod-discount");
            const name = nameInput.value.trim();
            const price= priceInput.value.trim();
            const stock= stockInput.value.trim();
            const discount= discountInput.value.trim();
            if (!name) {
                markFieldError(nameInput, "Product name is required");
                hasError = true;
            }
            if (price === "" || parseFloat(price) <= 0) {
                markFieldError(priceInput, "Price must be greater than 0");
                hasError = true;
            }
            if (stock === "" || parseInt(stock, 10) < 0) {
                markFieldError(stockInput, "Stock cannot be negative");
                hasError = true;
            }
            if (discount !== "" && parseFloat(discount) < 0) {
                markFieldError(discountInput, "Discount cannot be negative");
                hasError = true;
            }
        });

        if (hasError) {
            e.preventDefault();
            return;
        }

        const hiddenContainer = document.getElementById("hiddenInputsContainer");
        hiddenContainer.innerHTML = "";

        cards.forEach((card, idx) => {
            const fields = {
                ProductId:      card.querySelector(".prod-id").value,
                ProductName:    card.querySelector(".prod-name").value.trim(),
                Category:       card.querySelector(".prod-category").value,
                Price:          card.querySelector(".prod-price").value,
                Discount:       card.querySelector(".prod-discount").value || "0",
                AvailableStock: card.querySelector(".prod-stock").value,
                SupplierId:     supplierId
            };
            Object.keys(fields).forEach(key => {
                const input = document.createElement('input');
                input.type  = 'hidden';
                input.name  = `Products[${idx}].${key}`;
                input.value = fields[key];
                hiddenContainer.appendChild(input);
            });
        });

        if (isEdit) {
            deletedProductIds.forEach((id, idx) => {
                const input = document.createElement('input');
                input.type  = 'hidden';
                input.name  = `DeletedProductIds[${idx}]`;
                input.value = id;
                hiddenContainer.appendChild(input);
            });
        }
    });

    updateProductCountLabel();
}