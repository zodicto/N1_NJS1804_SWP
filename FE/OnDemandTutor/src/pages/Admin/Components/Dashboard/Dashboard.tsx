import { mdiAccountMultiple, mdiCartOutline } from '@mdi/js'
import CardBox from '../CardBox'
import ChartCard from '../ChartCard'
import { adminAPI } from '../../../../api/admin.api'
import { useEffect, useState } from 'react'
import { ChartData } from '../../../../types/chart.type'
import { toast } from 'react-toastify'

export default function Dashboard() {
  const [amountStudents, setAmountStudents] = useState<number>(0)
  const [amountTutors, setAmountTutors] = useState<number>(0)
  const [monthlyRevenue, setMonthlyRevenue] = useState<number>(0)
  const [revenueData, setRevenueData] = useState<ChartData[]>([])
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [students, tutors, revenue, yearlyRevenue] = await Promise.all([
          adminAPI.getAmountStudents(),
          adminAPI.getAmountTutors(),
          adminAPI.getMonthlyRevenue(),
          adminAPI.getRevenueByYear(new Date().getFullYear())
        ])
        setAmountStudents(students)
        setAmountTutors(tutors)
        setMonthlyRevenue(revenue)
        setRevenueData(yearlyRevenue || []) // Ensure yearlyRevenue is initialized as an array
        setError(null)
      } catch (error) {
        console.error('Lỗi khi lấy dữ liệu', error)
        setError('Không thể tải dữ liệu. Vui lòng thử lại sau.')
        // Hiển thị thông báo lỗi cho người dùng
        toast.error('Không thể tải dữ liệu. Vui lòng thử lại sau.')
        // Làm sạch state hoặc thực hiện các hành động khác nếu cần thiết
        setAmountStudents(0)
        setAmountTutors(0)
        setMonthlyRevenue(0)
        setRevenueData([]) // Reset to empty array on error
      }
    }

    fetchData()
  }, [])

  return (
    <div>
      <div className='flex gap-12 mb-10 w-11/12'>
        <CardBox
          icon={mdiAccountMultiple}
          iconColor='success'
          number={amountStudents}
          label='Học Sinh'
        />
        <CardBox
          icon={mdiAccountMultiple}
          iconColor='success'
          number={amountTutors}
          label='Gia sư'
        />
        <CardBox
          icon={mdiCartOutline}
          iconColor='success'
          number={monthlyRevenue}
          numberSuffix='VNĐ'
          label='Lợi nhuận của tháng này'
        />
      </div>
      <div className='w-11/12 bg-slate-50 p-3 rounded-lg shadow-md'>
        <div className='text-left font-bold mb-10'>Doanh Thu</div>
        {revenueData.length > 0 ? (
          <ChartCard dataChart={revenueData} />
        ) : (
          <p>Không có dữ liệu doanh thu để hiển thị.</p>
        )}
      </div>
    </div>
  )
}
