let wasInitialized = false;
let selectorsToDotnetHelpers = {};

window.dropdown = {
  handleOutsideClick,
  dispose: (dropdownSelector) => delete selectorsToDotnetHelpers[dropdownSelector]
}

function handleOutsideClick(dropdownSelector, dotnetHelper) {
  if (!wasInitialized) {
    window.addEventListener('click', async e => {
      for (const selector in selectorsToDotnetHelpers) {
        if (!e.target.closest(selector)) {
          await selectorsToDotnetHelpers[selector].invokeMethodAsync("OnOutsideClick");
        }
      }
    });
  }
  wasInitialized = true;
  selectorsToDotnetHelpers[dropdownSelector] = dotnetHelper;
}