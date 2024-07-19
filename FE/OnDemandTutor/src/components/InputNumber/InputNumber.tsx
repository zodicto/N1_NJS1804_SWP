import { InputHTMLAttributes, forwardRef, useState, useEffect } from 'react'

export interface InputNumberProps
  extends InputHTMLAttributes<HTMLInputElement> {
  errorMessage?: string
  classNameInput?: string
  classNameError?: string
  placeholder?: string
  inputType?: 'number' | 'price' | 'phone'
}

export const InputNumber = forwardRef<HTMLInputElement, InputNumberProps>(
  function InputNumberInner(
    {
      errorMessage,
      className,
      placeholder,
      classNameInput = 'p-3 w-full outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl',
      classNameError = 'mt-1 text-red-600 min-h-[1.25rem] text-sm text-left',
      onChange,
      value = '',
      inputType = 'number',
      ...rest
    },
    ref
  ) {
    const [localValue, setLocalValue] = useState<string>(value as string)

    useEffect(() => {
      setLocalValue(value as string)
    }, [value])

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
      let value = event.target.value

      if (inputType === 'price') {
        value = value.replace(/,/g, '')
      }

      const numberRegex = /^\d*$/
      const moneyRegex = /^(\d{1,3}(,\d{3})*|(\d+))(\.\d{0,2})?$/
      const phoneRegex = /^[0-9-+() ]*$/

      if (
        (inputType === 'number' && numberRegex.test(value)) ||
        (inputType === 'price' && moneyRegex.test(value)) ||
        (inputType === 'phone' && phoneRegex.test(value)) ||
        value === ''
      ) {
        setLocalValue(value)
        onChange && onChange(event)
      }
    }

    const formatMoney = (value: string) => {
      if (!value) return ''
      const parts = value.split('.')
      parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',')
      return parts.join('.')
    }

    const handleBlur = () => {
      if (inputType === 'price') {
        setLocalValue((prev) => formatMoney(prev))
      }
    }

    return (
      <div className={className}>
        <input
          placeholder={placeholder}
          className={classNameInput}
          {...rest}
          onChange={handleChange}
          onBlur={handleBlur}
          value={localValue}
          ref={ref}
        />
        <div className={classNameError}>{errorMessage}</div>
      </div>
    )
  }
)
