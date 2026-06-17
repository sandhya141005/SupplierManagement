
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
