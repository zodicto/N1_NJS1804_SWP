import React from 'react'

interface Props {
  onChange?: (value: string) => void // Accepts a string instead of string[]
  value: string // Change from string[] to string
  errorMessage?: string
}

const daysOfWeek = [
  { key: 'Thứ hai', label: 'Thứ hai' },
  { key: ' Thứ ba', label: 'Thứ ba' },
  { key: ' Thứ tư', label: 'Thứ tư' },
  { key: ' Thứ năm', label: 'Thứ năm' },
  { key: ' Thứ sáu', label: 'Thứ sáu' },
  { key: 'Thứ bảy', label: 'Thứ bảy' },
  { key: 'Chủ nhật', label: 'Chủ nhật' }
]

export default function DateOfWeek({
  onChange,
  value = '',
  errorMessage
}: Props) {
  const handleChange = (
    event: React.ChangeEvent<HTMLInputElement>,
    day: string
  ) => {
    const isChecked = event.target.checked
    let updatedDays = value.split(',').filter(Boolean) // Split and filter out empty strings

    if (isChecked && !updatedDays.includes(day)) {
      updatedDays.push(day) // Add day to array if it's not already included
    } else {
      updatedDays = updatedDays.filter((d) => d !== day) // Remove day from array
    }

    const concatenatedDays = updatedDays.join(',') // Concatenate with comma
    onChange?.(concatenatedDays) // Call onChange with concatenated string
  }

  return (
    <div className='container border-2 rounded-lg  flex flex-wrap flex-col'>
      <div className='pt-3 mb-2 text-left text-gray-400 text-sm'>
        Chọn các ngày trong tuần
      </div>
      <div className='flex flex-wrap'>
        {daysOfWeek.map((day) => (
          <label key={day.key} className='mr-3'>
            <input
              type='checkbox'
              checked={value.split(',').includes(day.key)} // Check if day is in list
              onChange={(e) => handleChange(e, day.key)}
              className='mr-1 cursor-pointer'
            />
            {day.label}
          </label>
        ))}
      </div>
      {errorMessage && (
        <div className='text-center mt-1 text-red-600 min-h-[1.25rem] text-sm '>
          {errorMessage}
        </div>
      )}
    </div>
  )
}
