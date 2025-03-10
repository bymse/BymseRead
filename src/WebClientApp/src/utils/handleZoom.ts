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
      // If there are exactly two touches, record the initial distance.
      if (event.touches.length === 2) {
        initialDistance = getDistance(event.touches[0], event.touches[1])
      }
    },
    { passive: true },
  )

  window.addEventListener(
    'touchmove',
    (event: TouchEvent) => {
      if (event.touches.length === 2 && initialDistance !== null) {
        event.preventDefault()
        const currentDistance = getDistance(event.touches[0], event.touches[1])
        const diff = currentDistance - initialDistance
        if (Math.abs(diff) > 5) {
          if (diff > 0) {
            zoomInCallback(2)
          } else {
            zoomOutCallback(2)
          }
          // Update the reference distance for continuous zoom gesture detection.
          initialDistance = currentDistance
        }
      }
    },
    { passive: true },
  )

  window.addEventListener('touchend', (event: TouchEvent) => {
    if (event.touches.length < 2) {
      initialDistance = null
    }
  })
}
