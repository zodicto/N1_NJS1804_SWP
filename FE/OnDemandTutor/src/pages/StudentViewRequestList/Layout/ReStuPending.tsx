import NavStudentRequestList from '../Nav/NavStudentRequestList'
import { RequestStudentPending } from '../RequestStudentPending/RequestStudentPending'

export default function ReStuPending() {
  return (
    <div className='w-4/5'>
      <NavStudentRequestList />
      <div className='border-2 shadow-xl'>
        <RequestStudentPending />
      </div>
    </div>
  )
}
