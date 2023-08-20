window.scrollGridTop = () => document.getElementById("books-grid").scrollTop = 0;
window.scrollToStateHeader = (state) => {
  const header = document.getElementById(state);
  header.scrollIntoView();
}