import { useEffect, useRef } from 'preact/hooks'

interface CroppedImageProps {
  src: string
  width: number
  height: number
  className?: string
}

export const CroppedImage = ({ src, width, height, className }: CroppedImageProps) => {
  const canvasRef = useRef<HTMLCanvasElement>(null)

  useEffect(() => {
    const canvas = canvasRef.current
    if (!canvas) return
    const ctx = canvas.getContext('2d')
    if (!ctx) return

    const dpr = window.devicePixelRatio || 1
    canvas.width = width * dpr
    canvas.height = height * dpr
    canvas.style.width = `${width}px`
    canvas.style.height = `${height}px`

    ctx.scale(dpr, dpr)

    ctx.imageSmoothingEnabled = true

    const img = new Image()
    img.src = src

    img.onload = () => {
      const scale = Math.max(width / img.width, height / img.height)
      const sWidth = width / scale
      const sHeight = height / scale

      ctx.drawImage(img, 0, 0, sWidth, sHeight, 0, 0, width, height)
    }
  }, [src, width, height])

  return <canvas ref={canvasRef} className={className} />
}
