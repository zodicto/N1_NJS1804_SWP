import { range } from 'lodash'
import { useEffect, useState } from 'react'

interface Props {
  onChange?: (value: Date) => void
  value?: Date
  name: string
  errorMessage?: string
}

export default function DateSelect({
  value,
  onChange,
  errorMessage,
  name
}: Props) {
  const [date, setDate] = useState({
    date: value?.getDate() || 1,
    month: value?.getMonth() || 0,
    year: value?.getFullYear() || 1990
  })

  useEffect(() => {
    if (value) {
      setDate({
        date: value.getDate(),
        month: value.getMonth(),
        year: value.getFullYear()
      })
    }
  }, [value])

  const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const { value: newValue, name } = event.target
    const newDate = {
      ...date,
      [name]: Number(newValue)
    }
    setDate(newDate)
    onChange && onChange(new Date(newDate.year, newDate.month, newDate.date))
  }

  return (
    <div className='flex flex-col'>
      <div className='pt-3 mb-2 text-left text-gray-400 text-sm'>{name}</div>
      <div>
        <div className='flex justify-between'>
          <select
            onChange={handleChange}
            name='date'
            className='h-10 w-[32%] rounded-md border border-black/10 px-3 cursor-pointer hover:border-black'
            value={date.date}
            aria-label='Ngày'
          >
            <option disabled>Ngày</option>
            {range(1, 32).map((item) => (
              <option value={item} key={item}>
                {item}
              </option>
            ))}
          </select>
          <select
            onChange={handleChange}
            name='month'
            className='h-10 w-[32%] rounded-md border border-black/10 px-3 cursor-pointer hover:border-black'
            value={date.month}
            aria-label='Tháng'
          >
            <option disabled>Tháng</option>
            {range(0, 12).map((item) => (
              <option value={item} key={item}>
                {item + 1}
              </option>
            ))}
          </select>
          <select
            onChange={handleChange}
            name='year'
            className='h-10 w-[32%] rounded-md border border-black/10 px-3 cursor-pointer hover:border-black'
            value={date.year}
            aria-label='Năm'
          >
            <option disabled>Năm</option>
            {range(1990, 2025).map((item) => (
              <option value={item} key={item}>
                {item}
              </option>
            ))}
          </select>
        </div>
        {errorMessage && (
          <div className='mt-1 text-red-600 min-h-[1.25rem] text-sm text-left'>
            {errorMessage}
          </div>
        )}
      </div>
    </div>
  )
}
