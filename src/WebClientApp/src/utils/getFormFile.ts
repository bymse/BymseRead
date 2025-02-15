export const getFormFile = (form: FormData, inputName: string): File | undefined => {
  const file = form.get(inputName) as File
  return file.name ? file : undefined
}
