import React, { useState, forwardRef, ForwardedRef } from 'react'

interface TypeSelectProps {
  onChange?: (value: string) => void
  value: string
  errorMessage?: string
  className: string
  classNameError: string
}

const TypeSelect = forwardRef(
  (props: TypeSelectProps, ref: ForwardedRef<HTMLSelectElement>) => {
    const { value, onChange, errorMessage, classNameError } = props
    const [type, setType] = useState(value || '')

    const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
      const { value } = event.target
      setType(value)
      onChange && onChange(value)
    }

    return (
      <div className='flex flex-wrap flex-col content-center mt-3'>
        <select
          ref={ref}
          onChange={handleChange}
          className='h-10 rounded-md border w-[300px] border-black/10 px-3 cursor-pointer hover:border-black'
          value={type}
        >
          <option value=''>Loại</option>
          <option value='cetificate'>Chứng Chỉ</option>
          <option value='bangCap'>Bằng Cấp</option>
        </select>
        <div className={classNameError}>{errorMessage}</div>
      </div>
    )
  }
)

export default TypeSelect
