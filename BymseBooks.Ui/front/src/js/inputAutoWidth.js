export function initializeInputAutoWidth(measurer, element) {
  setWidth(measurer, element);
  element.addEventListener('input', () => {
    setWidth(measurer, element);
  });
}

function setWidth(measurer, element) {
  measurer.textContent = element.value;
  element.style.width = `${measurer.clientWidth + 4}px`;
}

window.fixElementWidth = (measurerSelector, inputSelector) => {
  const measurer = document.querySelector(measurerSelector);
  const element = document.querySelector(inputSelector);
  setWidth(measurer, element);
}

window.initializeInputsAutoWidth = (measurerSelector, inputsSelector) => {
  const measurer = document.querySelector(measurerSelector);
  const elements = document.querySelectorAll(inputsSelector);
  for (let element of elements) {
    setWidth(measurer, element);
    element.addEventListener('input', () => {
      setWidth(measurer, element);
    });
  }
}