

window.openFullscreen = async (selector) => {
  await document.querySelector(selector).requestFullscreen();
}

window.exitFullscreen = async () => {
  await document.exitFullscreen();
}