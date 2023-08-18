// Получаем все элементы checkbox
const checkboxes = document.querySelectorAll('input[type=checkbox]');

// Функция для обработки события изменения состояния checkbox
function handleCheckboxChange(e) {
  // Получаем значение checkbox
  const value = e.target.value;

  // Получаем массив выбранных значений из localStorage, если он существует, или создаем пустой массив
  const selectedValues = JSON.parse(localStorage.getItem('selectedValues')) || [];

  // Если checkbox выбран, добавляем его значение в массив выбранных значений, иначе удаляем его значение из массива
  if (e.target.checked) {
    selectedValues.push(value);
  } else {
    const index = selectedValues.indexOf(value);
    if (index > -1) {
      selectedValues.splice(index, 1);
    }
  }

  // Сохраняем массив выбранных значений в localStorage
  localStorage.setItem('selectedValues', JSON.stringify(selectedValues));
}

// Добавляем обработчик события изменения состояния для каждого checkbox
checkboxes.forEach(checkbox => {
  checkbox.addEventListener('change', handleCheckboxChange);
});

// При загрузке страницы устанавливаем состояние checkbox на основе массива выбранных значений в localStorage
const savedValues = JSON.parse(localStorage.getItem('selectedValues')) || [];
checkboxes.forEach(checkbox => {
  if (savedValues.includes(checkbox.value)) {
    checkbox.checked = true;
  }
});
