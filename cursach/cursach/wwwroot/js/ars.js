// При изменении выбора Available, Reserved или Sold
document.querySelectorAll('select').forEach(item => {
    item.addEventListener('change', ()=>{
        var statusSelect = document.getElementById("Status");

        // Получаем текущие значения выбранных элементов
        var available = document.querySelector('select[name="AvailableId"]').value;
        var reserved = document.querySelector('select[name="ReservedId"]').value;
        var sold = document.querySelector('select[name="SoldId"]').value;

            // Определяем, что выбрать для Status
        if (available) {
            statusSelect.value = "Available";
        }
        else if (reserved) {
            statusSelect.value = "Reserved";
        }
        else if (sold) {
            statusSelect.value = "Sold";
        }
    });
});