let asp = document.querySelectorAll(".asp");
let button = document.querySelector(".submit");
let checkall = document.querySelector(".all");


asp.forEach(card => {
    card.addEventListener("click", () => {
        let check = card.querySelector("input");
        let img = card.querySelector(".checkbox_img");
        let input = document.getElementById('ids');
        input.classList.toggle(card.id);
        check.checked = !check.checked;
        if (check.checked == true) {
            img.style.visibility = "visible";
        }
        else {
            img.style.visibility = "hidden";
        }
    });
});

button.addEventListener("click", () => {
    var input = document.getElementById('ids');
    input.value = input.classList;
    document.getElementById('AddDoc').submit();
});

checkall.addEventListener("click", () => {
    asp.forEach(card => {
        card.click();
    });
});