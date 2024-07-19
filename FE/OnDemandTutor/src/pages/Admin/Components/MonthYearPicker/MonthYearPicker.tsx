import React, { useState, useEffect } from 'react';

interface MonthYearPickerProps {
  selectedDate: Date | null; // ngày được chọn có thể là null
  onChange: (date: Date | null) => void; //Hàm callback được gọi khi ngày thay đổi, nhận vào một giá trị Date.
}

const months = [ // định nghĩa tháng nè
  'Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4',
  'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8',
  'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'
];

export default function MonthYearPicker({ selectedDate, onChange }:MonthYearPickerProps)  {
  const [selectedMonth, setSelectedMonth] = useState<number>(selectedDate ? selectedDate.getMonth() : new Date().getMonth());//state sử dụng để chọn ngày, khởi tạo là ngày tháng hiện tại
  const [selectedYear, setSelectedYear] = useState<number>(selectedDate ? selectedDate.getFullYear() : new Date().getFullYear());

  useEffect(() => {
    const newDate = new Date(selectedYear, selectedMonth);
    onChange(newDate);
  }, [selectedMonth, selectedYear]);

  const handleMonthChange = (event: React.ChangeEvent<HTMLSelectElement>) => {// hàm sử lí thay đổi tháng
    setSelectedMonth(parseInt(event.target.value, 10));
  };

  const handleYearChange = (event: React.ChangeEvent<HTMLInputElement>) => {// hàm sử lí thay đổi năm
    setSelectedYear(parseInt(event.target.value, 10));
  };

  return (
    <div style={{ display: 'flex', alignItems: 'center' }}>
      <select value={selectedMonth} onChange={handleMonthChange}>
        {/* hàm trả về giá trị tháng  */}
        {months.map((month, index) => (
          <option key={index} value={index}>{month}</option>
        ))}
      </select>
      <input
        type="number"
        value={selectedYear}
        onChange={handleYearChange}
        style={{ marginLeft: '10px', width: '80px' }}
      />
    </div>
  );
};