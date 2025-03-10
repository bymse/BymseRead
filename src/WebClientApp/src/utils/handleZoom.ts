export function handleZoom(zoomInCallback: (ticks?: number) => void, zoomOutCallback: (ticks?: number) => void): void {
  window.addEventListener(
    'wheel',
    (event: WheelEvent) => {
      if (event.ctrlKey) {
        event.preventDefault()
        if (event.deltaY < 0) {
          zoomInCallback(2)
        } else if (event.deltaY > 0) {
          zoomOutCallback(2)
        }
      }
    },
    { passive: false },
  )

  let initialDistance: number | null = null

  function getDistance(touch1: Touch, touch2: Touch): number {
    const dx = touch1.pageX - touch2.pageX
    const dy = touch1.pageY - touch2.pageY
    return Math.hypot(dx, dy)
  }

  window.addEventListener(
    'touchstart',
    (event: TouchEvent) => {
      if (event.touches.length === 2) {
        initialDistance = getDistance(event.touches[0], event.touches[1])
      }
    },
    { passive: false },
  )

  window.addEventListener(
    'touchmove',
    (event: TouchEvent) => {
      if (event.touches.length === 2 && initialDistance !== null) {
        event.preventDefault()

        const currentDistance = getDistance(event.touches[0], event.touches[1])

        if (Math.abs(currentDistance - initialDistance) > 5) {
          if (currentDistance > initialDistance) {
            zoomInCallback(1)
          } else {
            zoomOutCallback(1)
          }
          // Update the reference distance for continuous gesture detection.
          initialDistance = currentDistance
        }
      }
    },
    { passive: false },
  )

  window.addEventListener(
    'touchend',
    (event: TouchEvent) => {
      if (event.touches.length < 2) {
        initialDistance = null
      }
    },
    { passive: false },
  )
}
