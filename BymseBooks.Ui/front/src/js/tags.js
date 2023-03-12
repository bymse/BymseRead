const TAG_INPUTS_SELECTOR = "[data-tag-input]";

const initializedTags = [];

window.initializeTagsInputs = () => {
  const measurer = document.querySelector("#tag-width-measurer");
  const elements = document.querySelectorAll(TAG_INPUTS_SELECTOR);
  for (let element of elements) {
    if (initializedTags.includes(element)) {
      continue;
    }

    setWidth(measurer, element);
    element.addEventListener('input', () => {
      setWidth(measurer, element);
    });
    element.addEventListener('keydown', handleKeyDown);
    initializedTags.push(element);
  }
}

function setWidth(measurer, element) {
  measurer.textContent = element.value;
  element.style.width = `${measurer.clientWidth + 4}px`;
}

function handleKeyDown(event) {
  const isLeft = event.keyCode === 37;
  const isRight = event.keyCode === 39;
  if (isLeft || isRight) {
    moveTagInputFocus(event, isLeft);
  }

  const isBackspace = event.keyCode === 8;
  if (isBackspace) {
    moveTagInputFocus(event, true);
  }
}

const moveTagInputFocus = (event, isLeft) => {
  const activeElement = event.target;
  const elements = document.querySelectorAll(TAG_INPUTS_SELECTOR);
  const index = Array.from(elements).indexOf(activeElement);
  if (index < 0) {
    return;
  }

  let newIndex = -1;
  let movedToLeft = false;
  if (activeElement.value.length === 0) {
    newIndex = isLeft ? index - 1 : index + 1;
    movedToLeft = isLeft;
  } else if (activeElement.selectionStart === activeElement.value.length) {
    newIndex = isLeft ? newIndex : index + 1;
    movedToLeft = false;
  } else if (activeElement.selectionStart === 0) {
    newIndex = isLeft ? index - 1 : newIndex;
    movedToLeft = true;
  }

  if (newIndex >= 0 && newIndex < elements.length) {
    event.preventDefault();
    elements[newIndex].focus();
    if (movedToLeft) {
      const len = document.activeElement.value.length;
      document.activeElement.setSelectionRange(len, len);
    } else {
      document.activeElement.setSelectionRange(0, 0);
      document.activeElement.selectionStart = 0;
    }
  }
}