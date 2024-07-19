import NavStudentRequestList from '../Nav/NavStudentRequestList'
import RequestStudentCurrent from '../RequestStudentCurrent/RequestStudentCurrent'

export default function ReStuCurrentPage() {
  return (
    <div className='w-4/5'>
      <NavStudentRequestList />
      <div className=' border-2 shadow-xl'>
        <RequestStudentCurrent />
      </div>
    </div>
  )
}
