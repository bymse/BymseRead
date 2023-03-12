
window.handleOutsideClick = (dotnetHelper) => {
  window.addEventListener('click', e => {
    if (!e.target.closest('[data-top-menu]')) {
      dotnetHelper.invokeMethodAsync("OnOutsideClick")
    }
  })
}