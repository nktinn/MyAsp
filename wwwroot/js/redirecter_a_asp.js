const changer = document.querySelector(".changer");
let input = changer.querySelector("input");
let button = changer.querySelector("button");

const hidden_div = document.querySelector(".hidden_div");
const changerpopup = document.getElementById("changerpopup");

button.onclick = () => {
    if (input.value != '') {
        if (!input.value.includes(" ")) {
            const aspBlocks = document.querySelectorAll(".asp-block");
            let selectedBlock = null;
            aspBlocks.forEach(block => {
                const cNumberDiv = block.querySelector("#cnumber");
                if (cNumberDiv.textContent.trim() === input.value) {
                    selectedBlock = block;
                    return;
                }
            });

            if (selectedBlock) {
                const changerPopupInputs = changerpopup.querySelectorAll("input");
                changerPopupInputs[0].value = selectedBlock.querySelector("#id").textContent;
                changerPopupInputs[1].value = selectedBlock.querySelector("#FIO").textContent;
                changerPopupInputs[2].value = selectedBlock.querySelector("#cnumber").textContent;
                changerPopupInputs[3].value = selectedBlock.querySelector("#gnumber").textContent;
                changerPopupInputs[4].value = selectedBlock.querySelector("#stDir").textContent;
                changerPopupInputs[5].value = selectedBlock.querySelector("#stProf").textContent;
                changerPopupInputs[6].value = selectedBlock.querySelector("#theme").textContent;
                changerPopupInputs[7].value = selectedBlock.querySelector("#scientist").textContent;
                changerPopupInputs[8].value = selectedBlock.querySelector("#tel").textContent;
                changerPopupInputs[9].value = selectedBlock.querySelector("#email").textContent;


                var a = document.createElement('a');
                a.href = "#changerpopup";
                a.click();
                hidden_div.style.display = "none";
                console.log("toggle to none 1");
            } else {
                hidden_div.style.display = "none";
                console.log("toggle to none 2");
                alert("Аспирант с указанным номером удостоверения не найден!");
            }
        } else {
            alert("Введите номер удостоверения без пробелов!");
        }
    } else {
        alert("Введите номер удостоверения!");
    }
}