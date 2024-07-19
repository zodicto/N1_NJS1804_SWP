import React from 'react';

interface ScheduleItem {
  date: string;
  timeSlots: string[];
}

interface Props {
  index: number;
  item: ScheduleItem;
  weekDates: Date[];
  hours: string[];
  handleDateChange: (index: number, date: string) => void;
  handleScheduleChange: (date: string, timeSlot: string) => void;
  handleRemoveSchedule: (index: number) => void;
  getDayOfWeek: (dateString: string) => string;
  handlePreviousWeek: () => void;
  handleNextWeek: () => void;
}

const ScheduleItemForm: React.FC<Props> = ({
  index, item, weekDates, hours, handleDateChange, handleScheduleChange, handleRemoveSchedule, getDayOfWeek, handlePreviousWeek, handleNextWeek
}) => (
  <div className="mb-2 border">
    <div className="mt-2 flex justify-between mb-4">
          <button type="button" onClick={handlePreviousWeek} className="bg-gray-500 hover:bg-gray-600 text-white font-bold py-1 px-2 rounded focus:outline-none focus:shadow-outline mr-2">
            {"<"}
          </button>
          <div className="mt-2 space-x-2 space-y-2">
            {weekDates.map((date) => (
              <div key={date.toISOString()} className="inline-block text-center border rounded-sm ">
                <button
                  type="button"
                  onClick={() => handleDateChange(index, date.toISOString().split('T')[0])}
                  className={`bg-gray-300 hover:bg-gray-400 text-black font-bold py-1 px-2 rounded focus:outline-none focus:shadow-outline ${item.date === date.toISOString().split('T')[0] ? 'bg-gray-500' : ''}`}
                >
                  <div className=''>{date.toISOString().split('T')[0]}</div>
                <p className='bg-slate-50 rounded-md'>{getDayOfWeek(date.toISOString().split('T')[0])}</p>
                </button>
              </div>
            ))}
          </div>
          <button type="button" onClick={handleNextWeek} className="bg-gray-500 hover:bg-gray-600 text-white font-bold py-1 px-2 rounded focus:outline-none focus:shadow-outline">
            {">"}
          </button>
    </div>
    
    {/* <p className="font-semibold mb-1 mt-2">Selected Date: {item.date}</p> */}
    <hr />
    <div className='text-left mt-2 ml-2'>Chọn giờ bắt đầu:</div>
    <div className="space-x-1">
      {hours.map((hour) => (
        <button
          key={hour}
          type="button"
          onClick={() => handleScheduleChange(item.date, hour)}
          className={`bg-pink-200 border hover:bg-pink-500 text-white font-bold py-1 px-2 rounded focus:outline-none focus:shadow-outline ${item.timeSlots.includes(hour) ? 'bg-pink-600 border-stone-600 border' : ''}`}
        >
          {hour}
        </button>
      ))}
    </div>
    {/* <p className="mt-1">Selected Time Slots: {item.timeSlots.join(', ')}</p> */}
    <button
      type="button"
      onClick={() => handleRemoveSchedule(index)}
      className="bg-red-500 hover:bg-red-600 text-white font-bold py-1 px-2 rounded focus:outline-none focus:shadow-outline mt-2"
    >
      Xóa ngày này
    </button>
  </div>
);

export default ScheduleItemForm;
