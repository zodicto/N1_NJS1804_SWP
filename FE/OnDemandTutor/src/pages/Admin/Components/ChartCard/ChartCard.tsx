import React, { useState } from 'react'
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer
} from 'recharts'
import {
  addDays,
  format,
  startOfWeek,
  endOfWeek,
  startOfMonth,
  endOfMonth
} from 'date-fns'
import MonthYearPicker from '../MonthYearPicker'

interface ChartData {
  name: string
  date: string
  revenue: number
  month: number
  year: number
}

interface ChartCardProps {
  dataChart: ChartData[]
}

export default function ChartCard({ dataChart }: ChartCardProps) {
  const [startDate, setStartDate] = useState<Date | null>(new Date())
  const [period, setPeriod] = useState<'month' | 'week'>('month')
  const [data, setData] = useState<ChartData[]>(dataChart)

  const handleDateChange = (date: Date | null) => {
    setStartDate(date)
    if (date) {
      const newData = generateDataForPeriod(date, period)
      setData(newData)
    }
  }

  const handlePeriodChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const newPeriod = event.target.value as 'month' | 'week'
    setPeriod(newPeriod)
    if (startDate) {
      const newData = generateDataForPeriod(startDate, newPeriod)
      setData(newData)
    }
  }

  const generateDataForPeriod = (
    date: Date,
    period: 'month' | 'week'
  ): ChartData[] => {
    const newData: ChartData[] = []
    let currentDate = new Date(date)

    if (period === 'month') {
      for (let i = 0; i < 12; i++) {
        const monthStart = startOfMonth(currentDate)
        const monthEnd = endOfMonth(currentDate)
        const monthName = `Tháng ${currentDate.getMonth() + 1}`

        const monthData = dataChart.filter(
          (data) =>
            data.month === currentDate.getMonth() + 1 &&
            data.year === currentDate.getFullYear()
        )
        const totalRevenue = monthData.reduce(
          (acc, data) => acc + data.revenue,
          0
        )

        newData.push({
          name: monthName,
          date: `${format(monthStart, 'dd/MM/yyyy')} - ${format(
            monthEnd,
            'dd/MM/yyyy'
          )}`,
          revenue: totalRevenue,
          month: currentDate.getMonth() + 1,
          year: currentDate.getFullYear()
        })
        currentDate.setMonth(currentDate.getMonth() + 1)
      }
    } else if (period === 'week') {
      const selectedMonth = currentDate.getMonth() + 1
      const selectedYear = currentDate.getFullYear()
      currentDate = startOfMonth(currentDate)

      for (let i = 0; i < 4; i++) {
        const weekStart = currentDate
        const weekEnd = i < 3 ? addDays(weekStart, 6) : endOfMonth(weekStart)

        const weekName = `Tuần ${i + 1} của Tháng ${selectedMonth}`
        const weekData = dataChart.find(
          (data) =>
            data.date ===
              `${format(weekStart, 'dd/MM/yyyy')} - ${format(
                weekEnd,
                'dd/MM/yyyy'
              )}` &&
            data.month === selectedMonth &&
            data.year === selectedYear
        )

        newData.push({
          name: weekName,
          date: `${format(weekStart, 'dd/MM/yyyy')} - ${format(
            weekEnd,
            'dd/MM/yyyy'
          )}`,
          revenue: weekData ? weekData.revenue : 0,
          month: selectedMonth,
          year: selectedYear
        })

        currentDate = addDays(weekEnd, 1)
      }
    }
    return newData
  }

  const formatYAxis = (tickItem: any) => {
    if (tickItem >= 1000000) {
      return `${(tickItem / 1000000).toFixed(1)}tr`
    }
    if (tickItem >= 1000) {
      return `${(tickItem / 1000).toFixed(1)}k`
    }
    return tickItem
  }

  return (
    <div>
      <div
        style={{
          display: 'flex',
          justifyContent: 'space-between',
          marginBottom: '20px'
        }}
      >
        <MonthYearPicker selectedDate={startDate} onChange={handleDateChange} />
        <select value={period} onChange={handlePeriodChange}>
          <option value='month'>Tháng</option>
          <option value='week'>Tuần</option>
        </select>
      </div>
      <ResponsiveContainer width='100%' height={400}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray='3 3' />
          <XAxis dataKey='name' />
          <YAxis tickFormatter={formatYAxis} />
          <Tooltip />
          <Legend />
          <Bar dataKey='revenue' fill='#8884d8' />
        </BarChart>
      </ResponsiveContainer>
    </div>
  )
}
