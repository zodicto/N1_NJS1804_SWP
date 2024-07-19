import NavMyServiceRequest from '../../../components/NavMyServiceRequest/NavMyServiceRequest'
import BookedService from '../BookedService'

export default function MyService() {
  return (
    <>
      <div className='w-4/5'>
        <NavMyServiceRequest />
        <div className=' border-2 shadow-xl'>
          <BookedService />
        </div>
      </div>
    </>
  )
}
