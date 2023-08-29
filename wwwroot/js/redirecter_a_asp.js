const changer = document.querySelector(".changer");
let input = changer.querySelector("input");
let button = changer.querySelector("button");

const hidden_div = document.querySelector(".hidden_div");
const changerpopup = document.getElementById("changerpopup");
const directionSelector = changerpopup.querySelector(".popup_input[name='stDir']");

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
                changerPopupInputs[4].value = selectedBlock.querySelector("#theme").textContent;
                changerPopupInputs[5].value = selectedBlock.querySelector("#tel").textContent;
                changerPopupInputs[6].value = selectedBlock.querySelector("#email").textContent;

                const scientistSelect = changerpopup.querySelector(".popup_input[name='scientist']");
                const scientistText = selectedBlock.querySelector("#scientist").textContent;
                const options = Array.from(scientistSelect.options);
                const optionToUpdate = options[0];
                optionToUpdate.selected = true;
                const optionToSelect = options.find(item => item.value == scientistText);
                if (optionToSelect != undefined)
                    optionToSelect.selected = true;

                const dirSelect = changerpopup.querySelector(".popup_input[name='stDir']");
                const dirText = selectedBlock.querySelector("#stDir").textContent;
                const diroptions = Array.from(dirSelect.options);
                const diroptionToUpdate = diroptions[0];
                diroptionToUpdate.selected = true;
                const diroptionToSelect = diroptions.find(item => item.value === dirText);
                if (diroptionToSelect != undefined) {
                    diroptionToSelect.selected = true;
                }
                else {
                    const profSelect = changerpopup.querySelector(".popup_input[name='stProf']");
                    profSelect.innerHTML = '<option value="null">Сначала выберите направление</option>';
                }

                const event = new Event("change");
                dirSelect.dispatchEvent(event);

                var a = document.createElement('a');
                a.href = "#changerpopup";
                a.click();
                hidden_div.style.display = "none";
            } else {
                hidden_div.style.display = "none";
                alert("Аспирант с указанным номером удостоверения не найден!");
            }
        } else {
            alert("Введите номер удостоверения без пробелов!");
        }
    } else {
        alert("Введите номер удостоверения!");
    }
}

directionSelector.addEventListener("change", function () {

    const aspBlocks = document.querySelectorAll(".asp-block");
    let selectedBlock = null;
    aspBlocks.forEach(block => {
        const cNumberDiv = block.querySelector("#cnumber");
        if (cNumberDiv.textContent.trim() === input.value) {
            selectedBlock = block;
            return;
        }
    });

    const profText = selectedBlock.querySelector("#stProf").textContent;
    const profSelect = changerpopup.querySelector(".popup_input[name='stProf']");

    const dirText = directionSelector.value;
    const dirBlocks = document.querySelectorAll(".dir-block");
    profSelect.innerHTML = '<option value="null">Выберите профиль подготовки</option>';
    dirBlocks.forEach(block => {
        const dirName = block.querySelector("#name");
        if (dirName.textContent === dirText) {
            const profName = block.querySelector("#profile");
            const option = document.createElement("option");
            option.value = profName.textContent;
            option.text = profName.textContent;
            profSelect.appendChild(option);
        }
    });

    const profoptions = Array.from(profSelect.options);
    const profoptionToSelect = profoptions.find(item => item.value === profText);
    if (profoptionToSelect != undefined) {
        profoptionToSelect.selected = true;
    }
});