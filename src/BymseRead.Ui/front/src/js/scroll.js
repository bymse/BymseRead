window.scrollGridTop = () => document.getElementById("books-grid").scrollTop = 0;
window.scrollToStateHeader = (state) => {
  const header = document.getElementById(state);
  const {top: headerTop} = header.getBoundingClientRect();
  const grid = document.getElementById("books-grid");
  grid.scrollTop = grid.scrollTop + headerTop - 70;
}