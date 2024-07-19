import React, { useState, useEffect } from 'react'

interface TimeOfScheduleProps {
  value: { date: string; timeSlots: string[] }[]
  onChange: (value: { date: string; timeSlots: string[] }[]) => void
}

export default function TimeOfSchedule({
  value,
  onChange
}: TimeOfScheduleProps) {
  const [selectedTimes, setSelectedTimes] = useState<string[]>([])

  useEffect(() => {
    const selectedDate = value.find((v) => v.date === selectedTimes[0])
    if (selectedDate) {
      setSelectedTimes(selectedDate.timeSlots)
    }
  }, [value])

  const morningSlots = []
  const afternoonSlots = []

  for (let hour = 0; hour < 12; hour++) {
    for (let minute = 0; minute < 60; minute += 30) {
      const time = `${hour.toString().padStart(2, '0')}:${minute
        .toString()
        .padStart(2, '0')}`
      morningSlots.push({
        time,
        selected: selectedTimes.includes(time)
      })
    }
  }

  for (let hour = 12; hour < 24; hour++) {
    for (let minute = 0; minute < 60; minute += 30) {
      const time = `${hour.toString().padStart(2, '0')}:${minute
        .toString()
        .padStart(2, '0')}`
      afternoonSlots.push({
        time,
        selected: selectedTimes.includes(time)
      })
    }
  }

  const handleTimeClick = (time: string) => {
    let newSelectedTimes
    if (selectedTimes.includes(time)) {
      newSelectedTimes = selectedTimes.filter((t) => t !== time)
    } else {
      newSelectedTimes = [...selectedTimes, time]
    }
    setSelectedTimes(newSelectedTimes)
    onChange(
      value.map((v) =>
        v.date === selectedTimes[0] ? { ...v, timeSlots: newSelectedTimes } : v
      )
    )
  }

  const renderTimeSlots = (slots: { time: string; selected: boolean }[]) => (
    <div className='flex flex-wrap mt-2 mx-[2rem]'>
      {slots.map((slot, index) => (
        <div
          key={index}
          className={`px-2 py-2 border-2 mr-2 mb-2 cursor-pointer rounded-lg hover:bg-slate-200 transition duration-75 ${
            slot.selected ? 'bg-blue-300' : 'bg-white'
          }`}
          onClick={() => handleTimeClick(slot.time)}
        >
          {slot.time}
        </div>
      ))}
    </div>
  )

  return (
    <div className='mt-4 w-full mx-auto'>
      <label className='block text-gray-700 text-sm font-bold mb-2'>
        Thời gian lịch hẹn:
      </label>
      <div className='mb-4'>
        <h3 className='text-center font-bold'>Buổi sáng</h3>
        {renderTimeSlots(morningSlots)}
      </div>
      <div>
        <h3 className='text-center font-bold'>Buổi chiều</h3>
        {renderTimeSlots(afternoonSlots)}
      </div>
    </div>
  )
}
