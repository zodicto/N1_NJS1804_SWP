import { faPlus } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useContext, useState } from 'react'
import { roles } from '../../constant/roles'
import { AppContext } from '../../context/app.context'
import FormRequest from '../../pages/FormRequest/FormRequest'
import RequestList from '../../pages/RequestList'
import CreateService from '../../pages/Sevice/CreateSevice'
import ServiceList from '../../pages/Sevice/ServiceList'

const SidebarMenu = () => {
  const { profile } = useContext(AppContext)
  const [activePage, setActivePage] = useState('requestList')

  const [showForm, setShowForm] = useState(false)
  const [showFormService, setShowFormSerivce] = useState(false)
  const [showOptions, setShowOptions] = useState(false)

  const renderContent = () => {
    switch (activePage) {
      case 'requestList':
        return <RequestList />
      case 'serviceList':
        return <ServiceList />
      default:
        return <RequestList />
    }
  }

  //  mở form đăng ký yêu cầu
  const handleOpenPopup = () => {
    setShowOptions(false)
    setShowForm(!showForm)
  }

  // các lựa chọn
  const handleOption = () => {
    setShowOptions((prev) => !prev)
  }

  //  fomr mở dịch vụ
  const handleOpenPopupService = () => {
    setShowOptions(false)
    setShowFormSerivce(!showFormService)
  }

  //  đóng form (bấm hủy)
  const handleCloseForm = () => {
    setShowForm(false)
    setShowFormSerivce(false)
  }

  return (
    <div className='container min-h-96  w-full flex'>
      <div className='w-[15%] justify-start p-4 bg-slate-300 rounded-lg'>
        <button
          className={` py-2 px-4 mb-2 text-left rounded-xl transition-shadow ${
            activePage === 'requestList'
              ? ' translate-y-1 text-pink-500 bg-white  shadow-black transition-shadow duration-300 shadow-inner '
              : 'bg-white'
          }`}
          onClick={() => setActivePage('requestList')}
        >
          Danh sách yêu cầu của học sinh
        </button>

        <button
          className={`my-2 py-2 px-4 text-left  rounded-xl ${
            activePage === 'serviceList'
              ? ' translate-y-1 text-pink-500 bg-white  shadow-black transition-shadow duration-300 shadow-inner '
              : 'bg-white'
          }`}
          onClick={() => setActivePage('serviceList')}
        >
          Danh sách dịch vụ của gia sư
        </button>
      </div>
      <div className='w-3/4 p-4'>{renderContent()}</div>

      {profile?.roles.toLowerCase() !== roles.moderator &&
        profile?.roles.toLowerCase() !== roles.admin && (
          <div className='fixed bottom-6 right-6'>
            <div className='relative'>
              <button
                onClick={handleOption}
                className={`mb-24 bg-slate-500 text-white rounded-full p-4 shadow-lg hover:bg-transparent hover:text-black hover:shadow-xl transition-all duration-300 group ${
                  showOptions ? 'rotate-180' : ''
                }`}
                style={{ boxShadow: '0px 4px 10px rgba(0, 0, 0, 0.1)' }}
              >
                <FontAwesomeIcon icon={faPlus} />
              </button>
              <div
                className={` absolute bottom-24 right-0 bg-white p-2 shadow-lg rounded-lg overflow-hidden transition-all duration-300  ${
                  showOptions
                    ? 'translate-x-0 opacity-100 right-16'
                    : 'translate-x-full opacity-0'
                }`}
                style={{ width: '200px' }}
              >
                <div
                  onClick={handleOpenPopup}
                  className='mb-2 p-2 transform hover:translate-y-1 hover:shadow-inner hover:shadow-black transition-shadow rounded-xl   hover:text-pink-500'
                >
                  Tạo yêu cầu tìm gia sư
                </div>
                <div
                  onClick={handleOpenPopupService}
                  className='mb-2 p-2 transform hover:translate-y-1 hover:shadow-inner hover:shadow-black transition-shadow rounded-xl   hover:text-pink-500 '
                >
                  Tạo lớp cho gia sư
                </div>
              </div>
              {showForm && <FormRequest onClose={handleCloseForm} />}

              {showFormService && <CreateService onClose={handleCloseForm} />}
            </div>
          </div>
        )}
    </div>
  )
}

export default SidebarMenu
