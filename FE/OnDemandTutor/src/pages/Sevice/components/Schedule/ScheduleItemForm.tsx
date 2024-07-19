import React from 'react'

export default function ScheduleItemForm({
  index,
  item,
  weekDates,
  hours,
  handleDateChange,
  handleScheduleChange,
  handleRemoveSchedule,
  getDayOfWeek,
  errors
}: Props) {
  return (
    <div className='mt-4'>
      <div className='flex items-center'>
        <span className='mr-2'>{getDayOfWeek(item.date)}</span>
        <span className='mr-2'>{item.date}</span>
        <select
          className='mx-2 p-2 border rounded'
          value={item.selectedHour || ''}
          onChange={(e) =>
            handleScheduleChange(index, 'selectedHour', e.target.value)
          }
        >
          <option value=''>Chọn giờ</option>
          {hours.map((hour) => (
            <option key={hour} value={hour}>
              {hour}
            </option>
          ))}
        </select>
        <button
          className='bg-red-500 text-white px-2 py-1 rounded'
          onClick={() => handleRemoveSchedule(index)}
        >
          Xóa
        </button>
      </div>
      {errors && <div className='text-red-500 text-xs mt-1'>{errors}</div>}
    </div>
  )
}
