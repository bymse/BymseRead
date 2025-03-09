import styles from './CoverFileButton.module.css'
import { useRef, useState } from 'preact/hooks'
import { PlusIcon } from '@icons/PlusIcon.tsx'
import { CroppedImage } from '@components/CroppedImage/CroppedImage.tsx'
import { CloseCircleIcon } from '@icons/CloseCircleIcon.tsx'
import { ChangeIcon } from '@icons/ChangeIcon.tsx'

export type CoverFileButtonProps = {
  inputName: string
  removedCheckName: string
  coverUrl?: string | null
}

export const CoverFileButton = ({ inputName, removedCheckName, coverUrl }: CoverFileButtonProps) => {
  const [fileUrl, setFileUrl] = useState<string | undefined>(coverUrl || undefined)
  const [isRemoved, setIsRemoved] = useState<boolean>(false)
  const fileInputRef = useRef<HTMLInputElement>(null)

  const handleFileChange = (event: Event) => {
    const target = event.target as HTMLInputElement
    const file = target.files?.[0]
    if (file) {
      setFileUrl(URL.createObjectURL(file))
      setIsRemoved(false)
    }
  }

  const handleFileRemove = () => {
    setFileUrl(undefined)
    fileInputRef.current!.value = ''
    if (coverUrl) {
      setIsRemoved(true)
    }
  }

  return (
    <div className={styles.container}>
      <label className={styles.button}>
        {!fileUrl && (
          <>
            <PlusIcon />
            <div>Add cover</div>
          </>
        )}
        {fileUrl && (
          <>
            <ChangeIcon />
            <div>Change cover</div>
          </>
        )}
        <input
          type="file"
          name={inputName}
          style={{ display: 'none' }}
          onChange={handleFileChange}
          accept=".png, .jpg, .jpeg"
          ref={fileInputRef}
        />
      </label>
      {fileUrl && (
        <div className={styles.cover}>
          <span className={styles.close} onClick={handleFileRemove}>
            <CloseCircleIcon color="rgba(6,10,42,0.45)" />
          </span>
          <CroppedImage src={fileUrl} width={120} height={140} className={styles.canvas} />
        </div>
      )}
      <input type="checkbox" checked={isRemoved} style={{ display: 'none' }} name={removedCheckName} />
    </div>
  )
}
