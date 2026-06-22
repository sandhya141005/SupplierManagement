
let cartItems = [];

async function loadCart() {
    const userId = '@userId';
    if (!userId || userId === '0') return;
    const res = await fetch(`/Cart/GetCart`);
    if (res.ok) cartItems = await res.json();
    syncCartUI();
}

function syncCartUI() {
    cartItems.forEach(ci => {
        const display = document.querySelector(`.qty-display[data-product-id="${ci.productId}"]`);
        const btn = document.querySelector(`.add-to-cart-btn[data-product-id="${ci.productId}"]`);
        const badge = document.querySelector(`.in-cart-badge[data-product-id="${ci.productId}"]`);
        if (display) display.textContent = ci.quantity;
        if (badge) {
            badge.textContent = `Already in cart (${ci.quantity})`;
            badge.style.display = 'inline';
        }
        if (btn) btn.textContent = 'Update Cart';
    });
}

document.querySelectorAll('.qty-minus').forEach(btn => {
    btn.addEventListener('click', function () {
        const id = this.dataset.productId;
        const display = document.querySelector(`.qty-display[data-product-id="${id}"]`);
        let val = parseInt(display.textContent);
        if (val > 1) display.textContent = val - 1;
    });
});

document.querySelectorAll('.qty-plus').forEach(btn => {
    btn.addEventListener('click', function () {
        const id = this.dataset.productId;
        const display = document.querySelector(`.qty-display[data-product-id="${id}"]`);
        const stock = parseInt(document.querySelector(`.add-to-cart-btn[data-product-id="${id}"]`).dataset.stock);
        let val = parseInt(display.textContent);
        if (val < stock) display.textContent = val + 1;
        else showCartError("Quantity exceeds available stock.");
    });
});

document.querySelectorAll('.add-to-cart-btn').forEach(btn => {
    btn.addEventListener('click', async function () {
        const id = this.dataset.productId;
        const qty = parseInt(document.querySelector(`.qty-display[data-product-id="${id}"]`).textContent);
        const stock = parseInt(this.dataset.stock);

        if (qty < 1 || qty > stock) {
            showCartError("Quantity exceeds available stock.");
            return;
        }

        const res = await fetch('/Cart/AddToCart', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                productId: parseInt(id),
                productName: this.dataset.productName,
                supplierName: this.dataset.supplier,
                price: parseFloat(this.dataset.price),
                discount: parseFloat(this.dataset.discount),
                quantity: qty,
                availableStock: stock
            })
        });

        if (!res.ok) {
            const err = await res.text();
            showCartError(err);
            return;
        }

        const data = await res.json();
        const badge = document.querySelector(`.in-cart-badge[data-product-id="${id}"]`);
        if (badge) {
            badge.textContent = `In cart (${qty})`;
            badge.style.display = 'inline';
        }
        this.textContent = 'Update Cart';
        showCartSuccess(`${this.dataset.productName} added! (${data.count} item(s) in cart)`);
        cartItems = cartItems.filter(c => c.productId !== parseInt(id));
        cartItems.push({ productId: parseInt(id), quantity: qty });
    });
});

function showCartError(msg) {
    const box = document.getElementById('cartErrorBox');
    box.textContent = msg;
    box.style.display = 'block';
    setTimeout(() => box.style.display = 'none', 3000);
}

function showCartSuccess(msg) {
    const box = document.getElementById('cartSuccessBox');
    box.textContent = msg;
    box.style.display = 'block';
    setTimeout(() => box.style.display = 'none', 3000);
}

loadCart();
