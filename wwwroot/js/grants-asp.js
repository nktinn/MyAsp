let asp = document.querySelectorAll(".asp");
let button = document.querySelector(".submit");
let stip = document.querySelectorAll(".stip");
let checkall = document.querySelector(".all");

const today = new Date();
const formattedDate = today.toISOString().split('T')[0];
const inputElements = document.querySelectorAll(".d");
inputElements.forEach((input) => {
    input.value = formattedDate;
});

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

stip.forEach(card => {
    card.addEventListener("click", () => {
        let check = card.querySelector("input");
        let img = card.querySelector(".checkbox_img");
        let input = document.getElementById('stips');
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
    const ips_input = document.getElementById('ids');
    ips_input.value = ips_input.classList;
    var stip_input = document.getElementById('stips');
    const stip_classlist = stip_input.classList;
    stip_input.value = '';
    Array.from(stip_classlist).forEach(classname => {
        var stip_block = document.getElementById(classname);
        var stip_amount = stip_block.querySelector(".v");
        var stip_date = stip_block.querySelector(".d");
        stip_input.value += classname + ' ';
        stip_input.value += stip_amount.value + ' ';
        stip_input.value += stip_date.value + ' ';
    });
    document.getElementById('AddDoc').submit();
});

checkall.addEventListener("click", () => {
    asp.forEach(card => {
        card.click();
    });
});