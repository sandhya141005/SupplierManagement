
function showCartError(msg) {
    const box = document.getElementById('cartPageError');
    box.textContent = msg;
    box.style.display = 'block';
    box.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    setTimeout(() => box.style.display = 'none', 3000);
}

function recalcTotal() {
    let grand = 0;
    document.querySelectorAll('.cart-qty-display').forEach(display => {
        const id = display.dataset.productId;
        const price = parseFloat(display.dataset.price);
        const discount = parseFloat(display.dataset.discount);
        const qty = parseInt(display.textContent);
        const line = (price - ((discount*price)/100)) * qty;
        const lineEl = document.querySelector(`.cart-line-total[data-product-id="${id}"]`);
        if (lineEl) lineEl.textContent = line.toFixed(2);
        grand += line;
    });
    const grandEl = document.getElementById('grandTotal');
    const confirmEl = document.getElementById('confirmTotal');
    if (grandEl) grandEl.textContent = '₹' + grand.toFixed(2);
    if (confirmEl) confirmEl.textContent = '₹' + grand.toFixed(2);
}

document.querySelectorAll('.qty-cart-btn').forEach(btn => {
    btn.addEventListener('click', async function () {
        const productId = parseInt(this.dataset.productId);
        const stock = parseInt(this.dataset.stock);
        const action = this.dataset.action;
        const display = document.querySelector(`.cart-qty-display[data-product-id="${productId}"]`);
        let qty = parseInt(display.textContent);

        if (action === 'plus') {
            if (qty >= stock) {
                showCartError("Quantity exceeds available stock.");
                return;
            }
            qty++;
        } else {
            if (qty <= 1) {
                if (!confirm('Remove this item from cart?')) return;
                const form = document.createElement('form');
                form.method = 'POST';
                form.action = `/Cart/Remove?productId=${productId}`;
                document.body.appendChild(form);
                form.submit();
                return;
            }
            qty--;
        }

        display.textContent = qty;
        recalcTotal();

        await fetch('/Cart/UpdateQty', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ productId, quantity: qty })
        });
    });
});