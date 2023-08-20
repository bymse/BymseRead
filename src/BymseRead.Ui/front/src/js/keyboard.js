
window.onEscapeKeyDown = (dotnetHelper, method) => {
  document.addEventListener('keydown', e => {
    if (e.key === 'Escape') {
      e.preventDefault();
      dotnetHelper.invokeMethodAsync(method);
    }
  })
}