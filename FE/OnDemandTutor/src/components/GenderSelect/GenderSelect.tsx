import React, { useEffect, useState } from 'react'

interface Props {
  onChange?: (value: string) => void
  value: string
  errorMessage?: string
}

export default function GenderSelect({ value, onChange, errorMessage }: Props) {
  // Initialize gender state with 'nam'
  const [gender, setGender] = useState(value || 'nam')

  useEffect(() => {
    if (value) {
      setGender(value)
    }
  }, [value])

  const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const { value: newValue } = event.target
    setGender(newValue)
    onChange && onChange(newValue)
  }

  return (
    <div className='flex flex-wrap flex-col'>
      <div className='pt-3 mb-2 text-left text-gray-400 text-sm'>Giới tính</div>
      <select
        onChange={handleChange}
        className='h-10 w-full rounded-md border border-black/10 px-3 cursor-pointer hover:border-black'
        value={gender}
      >
        <option value='nam'>Nam</option>
        <option value='nữ'>Nữ</option>
      </select>
      {errorMessage && (
        <div className='mt-1 text-red-600 min-h-[1.25rem] text-sm text-left'>
          {errorMessage}
        </div>
      )}
    </div>
  )
}
