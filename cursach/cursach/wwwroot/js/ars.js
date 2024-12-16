document.addEventListener("DOMContentLoaded", ()=>{
    document.getElementById('Status').addEventListener("change", function () {
        var Available = document.getElementById("Available");
        var Reserved = document.getElementById("Reserved");
        var Sold = document.getElementById("Sold");

        var select_status = this.value;
        console.log(select_status);

        if (select_status == "Available") {
            Available.style.display = "flex";
            Reserved.style.display = "none";
            Sold.style.display = "none";
        }
        else if (select_status == "Reserved") {
            Available.style.display = "none";
            Reserved.style.display = "flex";
            Sold.style.display = "none";
        }
        else if (select_status == "Sold") {
            Available.style.display = "none";
            Reserved.style.display = "none";
            Sold.style.display = "flex";
        }
    });
});