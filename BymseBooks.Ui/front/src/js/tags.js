

window.initializeTagsInputs = () => {
  const measurer = document.querySelector("#tag-width-measurer");
  const elements = document.querySelectorAll("[data-tag-input]");
  for (let element of elements) {
    setWidth(measurer, element);
    element.addEventListener('input', () => {
      setWidth(measurer, element);
    });
  }
}

function setWidth(measurer, element) {
  measurer.textContent = element.value || element.placeholder;
  element.style.width = `${measurer.clientWidth + 4}px`;
}