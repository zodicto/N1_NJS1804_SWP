import NavStudentRequestList from '../Nav/NavStudentRequestList'

import RequestStudentReject from '../RequestStudentReject/RequestStudentReject'

export default function ReStuReject() {
  return (
    <div className='w-4/5'>
      <NavStudentRequestList />
      <div className=' border-2 shadow-xl'>
        <RequestStudentReject />
      </div>
    </div>
  )
}
