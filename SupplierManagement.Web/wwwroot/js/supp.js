function initializeSupplierForm(options) {
    isEdit = options.isEdit;
    supplierId = options.supplierId;
    deletedProductIds = [];

    // ── Helpers ───────────────────────────────────────────────────
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

    function clearInlineErrors() {
        document.querySelectorAll('.prod-field-error').forEach(el => el.remove());
        document.querySelectorAll('.input-error').forEach(el => el.classList.remove('input-error'));
    }

    function markFieldError(input, message) {
        if (!input) return;
        input.classList.add('input-error');
        const existing = input.parentElement.querySelector('.prod-field-error');
        if (existing) existing.remove();
        const span = document.createElement('span');
        span.className = 'prod-field-error';
        span.style.cssText = 'color:#B71C1C;font-size:0.8rem;display:block;margin-top:2px;';
        span.textContent = message;
        input.insertAdjacentElement('afterend', span);
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

    // ── Cascading dropdowns ───────────────────────────────────────
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
                    opt.value = s.value; opt.text = s.text;
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
                    opt.value = c.value; opt.text = c.text;
                    cityDropdown.appendChild(opt);
                });
            });
    });

    // ── Add product card ──────────────────────────────────────────
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
                    <div class="form-group"><label>Product Name</label><input class="prod-name" placeholder="Product Name" /></div>
                    <div class="form-group"><label>Price</label><input class="prod-price" placeholder="Price" type="number" step="0.01" /></div>
                    <div class="form-group"><label>Discount</label><input class="prod-discount" placeholder="Discount" type="number" step="0.01" value="0" /></div>
                    <div class="form-group"><label>Available Stock</label><input class="prod-stock" placeholder="Stock" type="number" /></div>
                    <div class="form-group" style="grid-column:1/-1;"><label>Category</label><input class="prod-category" value="${category}" readonly /></div>
                    <div class="form-group"><label>Created Date</label><input class="prod-date" type="date"/></div>
                </div>
            </div>`;
        document.getElementById("productsContainer").insertAdjacentHTML("beforeend", html);
        /*const newCards = document.querySelectorAll('#productsContainer .product-card');
        const lastCard = newCards[newCards.length - 1];
        const newDateInput = lastCard.querySelector('.prod-date');
        if (newDateInput && typeof flatpickr !== 'undefined') {
            flatpickr(newDateInput, { dateFormat: "d-M-Y", allowInput: true });
        }*/
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
let hasAnyError = false;
    // ── Submit ────────────────────────────────────────────────────
    document.getElementById("supplierForm").addEventListener("submit", function (e) {
        // Always prevent default first — run ALL validations, then decide
       // e.preventDefault();
     //  let formIsValid = $("#supplierForm").valid();
       clearError();
        clearInlineErrors();

        // Track errors separately so nothing short-circuits
        let hasSupplierError = false;
        const countErrors = [];       // goes to error box
        let hasProductFieldError = false;
        let hasAnyError = false;

        // ── 1. Supplier fields ──────────────────────────────────
    /*    const companyInput  = document.getElementById("CompanyName");
        const catalogInput  = document.getElementById("CatalogType");
        const totalInput    = document.getElementById("TotalProducts");
        const dateInput     = document.getElementById("CreatedDate");
        const countryInput  = document.getElementById("CountryId");
        const stateInput    = document.getElementById("StateId");
        const cityInput     = document.getElementById("CityId");

        if (!companyInput.value.trim()) {
            markFieldError(companyInput, "Company name is required");
            hasSupplierError = true;
        }
        if (!catalogInput.value.trim()) {
            markFieldError(catalogInput, "Catalog type is required");
            hasSupplierError = true;
        }
        const total = parseInt(totalInput.value, 10);
        if (!totalInput.value || isNaN(total) || total < 1) {
            markFieldError(totalInput, "Total products is required and must be at least 1");
            hasSupplierError = true;
        }
        if (!dateInput || !dateInput.value.trim()) {
            markFieldError(dateInput, "Created date is required");
            hasSupplierError = true;
        } else {
            const dateRegex = /^\d{2}-[A-Za-z]{3}-\d{4}$/;
            if (!dateRegex.test(dateInput.value.trim())) {
                markFieldError(dateInput, "Date must be in dd-Mon-yyyy format e.g. 11-Jun-2026");
                hasSupplierError = true;
            }
        }
        if (!countryInput.value || countryInput.value === "0") {
            markFieldError(countryInput, "Please select a country");
            hasSupplierError = true;
        }
        if (!stateInput.value || stateInput.value === "0") {
            markFieldError(stateInput, "Please select a state");
            hasSupplierError = true;
        }
        if (!cityInput.value || cityInput.value === "0") {
            markFieldError(cityInput, "Please select a city");
            hasSupplierError = true;
        }

        const checkedPayments = document.querySelectorAll('input[name="SelectedPaymentMethods"]:checked');
        if (checkedPayments.length === 0) {
            const paymentBox = document.querySelector('.payment-checkboxes');
            markFieldError(paymentBox, "Select at least one payment method");
            hasSupplierError = true;
        }
*/
        // ── 2. Product count ────────────────────────────────────
        const total = parseInt(document.getElementById("TotalProducts").value, 10);
        const cards = document.querySelectorAll('#productsContainer .product-card');
        const cardCount = cards.length;

        if (cardCount === 0) {
            countErrors.push("At least one product is required.");
        } else if (!isNaN(total) && cardCount < total) {
            countErrors.push(`Add ${total - cardCount} more product(s) or update Total Products.`);
        } else if (!isNaN(total) && cardCount > total) {
            countErrors.push(`Remove ${cardCount - total} product(s) or update Total Products.`);
        }

        // ── 3. Product field validation (always runs if cards exist) ──
        cards.forEach((card) => {
            const nameInput     = card.querySelector(".prod-name");
            const priceInput    = card.querySelector(".prod-price");
            const stockInput    = card.querySelector(".prod-stock");
            const discountInput = card.querySelector(".prod-discount");
            const prodDateInput = card.querySelector(".prod-date");

            if (!nameInput.value.trim()) {
                markFieldError(nameInput, "Product name is required");
                hasProductFieldError = true;
            }
            if (!priceInput.value || parseFloat(priceInput.value) <= 0) {
                markFieldError(priceInput, "Price must be greater than 0");
                hasProductFieldError = true;
            }
            if (stockInput.value === "" || parseInt(stockInput.value, 10) < 0) {
                markFieldError(stockInput, "Stock cannot be negative");
                hasProductFieldError = true;
            }
            if (discountInput.value !== "" && parseFloat(discountInput.value) < 0) {
                markFieldError(discountInput, "Discount cannot be negative");
                hasProductFieldError = true;
            }
            // prod-date is a text input (flatpickr) on dynamically added cards
            // and a <input type="date"> on Razor-rendered edit cards
            if (prodDateInput) {
                if (!prodDateInput.value.trim()) {
                    markFieldError(prodDateInput, "Product date is required");
                    hasProductFieldError = true;
                }
            }
        });

        // ── 4. Show count/logic errors in error box ─────────────
        if (countErrors.length > 0) {
            showError(countErrors);
        }

        // ── 5. If ANY error exists, stop here ───────────────────
      const formIsValid = $("#supplierForm").valid();
console.log({
    total,
    cardCount,
    countErrors,
    formIsValid
});
if (!formIsValid)
{
    e.preventDefault();
}

if (countErrors.length > 0 || hasProductFieldError)
{
    e.preventDefault();
}

if (!formIsValid || countErrors.length > 0 || hasProductFieldError)
{
    return;
}

        // ── 6. All valid — build hidden inputs and submit ────────
        const hiddenContainer = document.getElementById("hiddenInputsContainer");
        hiddenContainer.innerHTML = "";
        const razorCount = document.querySelectorAll(
            '#productsContainer .product-card input.prod-id:not([value="0"])').length;
        let dynamicIdx = razorCount;

        cards.forEach((card) => {
            const productId = card.querySelector(".prod-id")?.value ?? "0";
            if (productId !== "0") return;
            const fields = {
                ProductId:      "0",
                ProductName:    card.querySelector(".prod-name").value.trim(),
                Category:       card.querySelector(".prod-category").value,
                Price:          card.querySelector(".prod-price").value,
                Discount:       card.querySelector(".prod-discount").value || "0",
                AvailableStock: card.querySelector(".prod-stock").value,
                CreatedDate:    card.querySelector(".prod-date")?.value ?? "",
                SupplierId:     supplierId
            };
            Object.keys(fields).forEach(key => {
                const input = document.createElement('input');
                input.type = 'hidden';
                input.name = `Products[${dynamicIdx}].${key}`;
                input.value = fields[key];
                hiddenContainer.appendChild(input);
            });
            dynamicIdx++;
        });

        if (isEdit) {
            deletedProductIds.forEach((id, idx) => {
                const input = document.createElement('input');
                input.type = 'hidden';
                input.name = `DeletedProductIds[${idx}]`;
                input.value = id;
                hiddenContainer.appendChild(input);
            });
        }

        // Actually submit now that everything is valid
        this.submit();
    });

    updateProductCountLabel();
}



