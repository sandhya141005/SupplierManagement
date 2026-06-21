 document.getElementById('searchBox').addEventListener('input', function () {
        const query = this.value.toLowerCase().trim();

        // Admin table rows
        const rows = document.querySelectorAll('.supplier-row');
        // User cards
        const cards = document.querySelectorAll('.supplier-card');
        const noResults = document.getElementById('noResults');

        let visibleCount = 0;

        rows.forEach(row => {
            const matches = row.dataset.company.includes(query) ||
                            row.dataset.category.includes(query);
            row.style.display = matches ? '' : 'none';
            if (matches) visibleCount++;
        });

        cards.forEach(card => {
            const matches = card.dataset.company.includes(query) ||
                            card.dataset.category.includes(query);
            card.style.display = matches ? '' : 'none';
            if (matches) visibleCount++;
        });

        if (noResults) {
            noResults.style.display = (rows.length + cards.length > 0 && visibleCount === 0)
                ? 'block' : 'none';
        }
    });