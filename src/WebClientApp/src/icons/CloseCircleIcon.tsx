import { FunctionalComponent } from 'preact'

type Props = {
  color?: string
}

export const CloseCircleIcon: FunctionalComponent<Props> = ({ color }: Props) => {
  return (
    <div style={{ minWidth: '18px', height: '18px' }}>
      <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 18 18" fill="none">
        <circle cx="9" cy="9" r="9" fill={color} />
        <path
          d="M7.93935 9L5.4696 6.53032C5.1768 6.23745 5.1768 5.76255 5.4696 5.46968C5.76255 5.1768 6.23745 5.1768 6.53032 5.46968L9 7.93935L11.4697 5.4696C11.7625 5.1768 12.2375 5.1768 12.5303 5.4696C12.8232 5.76255 12.8232 6.23745 12.5303 6.53032L10.0606 9L12.5304 11.4697C12.8232 11.7625 12.8232 12.2375 12.5304 12.5303C12.2375 12.8232 11.7625 12.8232 11.4697 12.5303L9 10.0606L6.53032 12.5304C6.23745 12.8232 5.76255 12.8232 5.46968 12.5304C5.1768 12.2375 5.1768 11.7625 5.46968 11.4697L7.93935 9Z"
          fill="white"
        />
      </svg>
    </div>
  )
}
