import {
  FloatingPortal,
  Placement,
  arrow,
  offset,
  shift,
  useFloating
} from '@floating-ui/react'
import { AnimatePresence, motion } from 'framer-motion'
import { ElementType, useId, useRef, useState } from 'react'

interface Props {
  children: React.ReactNode
  renderPopover: React.ReactNode
  className?: string
  //  as được dùng trong trường hợp mà người đùng muốn sử dụng thẻ kháC ngoài thẻ div
  as?: ElementType
  initalOpen?: boolean
  placement?: Placement
}

export default function Popover({
  children,
  className,
  renderPopover,
  as: Element = 'div',
  initalOpen,
  placement = 'bottom'
}: Props) {
  //  nà
  const [isOpen, setIsOpen] = useState(initalOpen || false)
  const id = useId()

  // cái này là đảm bảo là  cái mà chúng ta tham chiếu đến là 1 HTMLElement
  const arrowRef = useRef<HTMLElement>(null)

  //------------------------------------
  //  tọa độ x , y (trục dọc, trục ngáng)
  //  refs là phân từ được tham chiếu (phần từ gốc)
  // floatingStyles là phầ tử mình muốn nổi
  // strategy là Chiến lược định vị (absolute hoặc fixed).
  // middlewareData là dữ từ các middleWare

  const { x, y, refs, floatingStyles, strategy, middlewareData } = useFloating({
    //offset khoảng cách từ phần tử tham chiếu,
    //shift Đảm bảo phần tử nổi luôn nằm trong màn hình
    //arrow giúp cho mũit tên của phần từ tham chiếu sẽ chỉ vào phần tử nổ
    middleware: [offset(6), shift(), arrow({ element: arrowRef })],
    // vị trí phần tử nổi
    placement: placement
  })

  //  dùng để show đối tượng
  const showPopover = () => {
    setIsOpen(true)
  }

  const hidePopover = () => {
    setIsOpen(false)
  }
  return (
    <Element
      className={className}
      ref={refs.setReference}
      onMouseMove={showPopover}
      onMouseLeave={hidePopover}
    >
      {/*  chilren này là thằng mình muốn show mặc định  */}
      {children}
      {/*  thằng này bắt buộc phải bao bộc bằng thằng Portal trong doc  */}
      <FloatingPortal id={id}>
        <AnimatePresence>
          {isOpen && (
            <motion.div
              ref={refs.setFloating}
              style={{
                position: strategy,
                // tránh để cho x hoặc y mà bị null thì nó sẽ bằng 0
                top: y ?? 0,
                left: x ?? 0,
                width: 'max-content',
                ...floatingStyles,
                transformOrigin: `${middlewareData.arrow?.x}px-top`
              }}
            >
              {' '}
              <span
                ref={arrowRef}
                className='border-x-transparent border-t-transparent border-b-black border-[8px] absolute -translate-y-full'
                style={{
                  left: middlewareData.arrow?.x,
                  top: middlewareData.arrow?.y
                }}
              />
              {renderPopover}
            </motion.div>
          )}
        </AnimatePresence>
      </FloatingPortal>
    </Element>
  )
}
