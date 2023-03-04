

window.inputAutoResize = (measurerSelector, selector) => {
  const measurer = document.querySelector(measurerSelector);
  const elements = document.querySelectorAll(selector);
  for (let element of elements) {
    setWidth(measurer, element);
    element.addEventListener('input', () => {
      setWidth(measurer, element);
    });
  }
}

function setWidth(measurer, element) {
  measurer.textContent = element.value;
  element.style.width = `${measurer.clientWidth + 4}px`;
}