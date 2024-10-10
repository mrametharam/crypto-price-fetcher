const dataSelect = document.getElementById('dataSelect');
const fetchDataBtn = document.getElementById('fetchDataBtn');
const loadingMessage = document.getElementById('loadingMessage');

document.addEventListener('DOMContentLoaded', pageLoad);
fetchDataBtn.addEventListener('click', fetchDataBtnClicked);

let apiUrls = {};

async function pageLoad() {
    await loadConfig();

    var cryptoSymbols = await fetchApiData(apiUrls.cryptoSymbolsUrl);

    if (cryptoSymbols == null) {
        return;
    }

    // Populate dropdown with fetched data
    cryptoSymbols.forEach(item => {
        const option = document.createElement('option');
        option.value = item.symbol;
        option.textContent = item.symbol;

        dataSelect.appendChild(option);
    });
};

async function loadConfig() {
    apiUrls = await fetchApiData('/api/configurations');
}

// Function to fetch data from API
async function fetchApiData(api) {
    try {
        loadingMessage.style.display = 'block'; // Show loading message

        const response = await fetch(api); // Replace with your API endpoint
        const data = await response.json();

        dataSelect.disabled = false;
        fetchDataBtn.disabled = false;

        loadingMessage.style.display = 'none'; // Hide loading message
        return data;

    } catch (error) {
        console.error('Error fetching data:', error);
        loadingMessage.textContent = 'Error loading data. Please try again later.';

        return null;
    }

}

async function fetchDataBtnClicked() {
    const selectedDataSet = dataSelect.value;

    if (selectedDataSet == null || selectedDataSet == '') {
        alert('Select a valid crypto first');
        return;
    }

    var cryptoPrices = await fetchApiData(`${apiUrls.cryptoPriceUrl}${selectedDataSet}`);

    if (cryptoPrices == null) {
        alert('No data found for ' + selectedDataSet);
        return;
    }

    const dataTableBody = document.getElementById('dataTable').getElementsByTagName('tbody')[0];
    dataTableBody.innerHTML = ''; // Clear existing data

    // Populate dropdown with fetched data
    cryptoPrices.forEach(item => {
        const row = dataTableBody.insertRow();

        row.insertCell(0).textContent = item.id;
        row.insertCell(1).textContent = item.crypto;
        row.insertCell(2).textContent = item.price;
        row.insertCell(3).textContent = item.timeStamp;
    });
}
