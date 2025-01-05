type Props = {
  label: string
}

export const Button = ({label}: Props) => {
  return (
    <button
      type="button"
    >
      {label}
    </button>
  );
};