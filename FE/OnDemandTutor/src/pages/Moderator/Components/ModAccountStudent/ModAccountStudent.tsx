import Search from 'antd/es/transfer/search'
import ModMenu from '../ModMenu/ModMenu'
import { useEffect } from 'react'
import { Table, TableColumnsType } from 'antd'
import { useQuery } from '@tanstack/react-query'
import { moderatorApi } from '../../../../api/moderator.api'
import { User } from '../../../../types/user.type'

// interface DataType {
//   id:string,
//   fullName: string
//   email:string,
//   dateOfBirth: string
//   gender:string,
//   roles:string,
//   address:string,
//   phone:string
//   accountBalance:number
// }

export default function ModAccountStudent() {
  //Lấy danh sách yêu cầu từ API
  const { data: Data } = useQuery<User[]>({
    queryKey: ['Account'],
    queryFn: () => moderatorApi.getAccount()
  })
  useEffect(() => {
    if (Data) {
      console.log(Data)
    } else console.log('Data')
  }, [Data])
  // })
  const handleDelete = () => {}
  const columns: TableColumnsType<User> = [
    {
      title: 'Tên Học Sinh',
      dataIndex: 'fullName',
      onFilter: (value, record) =>
        record.fullName.indexOf(value as string) === 0,
      sorter: (a, b) => a.fullName.length - b.fullName.length,
      width: 200,
      fixed: 'left'
    },
    {
      title: 'Email',
      dataIndex: 'email',
      defaultSortOrder: 'descend',
      width: 200
    },
    {
      title: 'Ngày sinh',
      dataIndex: 'dateOfBirth',
      width: 200
    },
    {
      title: 'Giới tính',
      dataIndex: 'gender',
      width: 150
    },
    {
      title: 'Chức vụ',
      dataIndex: 'roles',
      width: 150
    },
    {
      title: 'Địa chỉ',
      dataIndex: 'address',
      width: 150
    },
    {
      title: 'Số điện thoại',
      dataIndex: 'phone',
      width: 150
    },
    {
      title: 'Số dư',
      dataIndex: 'accountBalance',
      width: 150
    },
    {
      title: 'Hành động',
      dataIndex: 'delete',
      fixed: 'right',
      className: 'TextAlign',
      width: 150,
      render: () => (
        <div className='flex gap-1'>
          <button
            className='p-1 border border-red-500 rounded-lg hover:bg-red-500 active:bg-red-700 w-[150px]'
            onClick={() => handleDelete()} // chưa có API
          >
            Xóa
          </button>
        </div>
      )
    }
  ]

  const onChange = () => {} // Placeholder for future implementation

  //const [selectedRecord, setSelectedRecord] = useState<User | null>(null)
  // const [visible, setVisible] = useState(false)

  // const showDetail = (id: string) => {
  //   const record = RequestData?.find((item) => item.idRequest === id) || null
  //   console.log('id', id)
  //   setSelectedRecord(record)
  //   setVisible(true)
  // }

  // const handleCancel = () => {
  //   setVisible(false)
  //   setSelectedRecord(null)
  // }

  return (
    <>
      <ModMenu kind='student' style='Option2' />
      <div className='text-left'>Quản lí tài khoản</div>
      <div className='text-left shadow-2xl shadow-black border-4 pt-5 h-[629px] rounded-t-xl mt-6'>
        <div className='mb-5'>
          <Search />
        </div>
        <Table
          columns={columns}
          dataSource={Data}
          pagination={{ pageSize: 10 }}
          onChange={onChange}
          showSorterTooltip={{ target: 'sorter-icon' }}
          scroll={{ x: 1300, y: 400 }}
        />
        {/* <Modal
          title='Chi tiết'
          visible={visible}
          onCancel={handleCancel}
          footer={[
            <Button key='approve' onClick={handleApprove}>
              Xác nhận
            </Button>,
            <Button key='reject' onClick={handleReject}>
              Từ chối
            </Button>
          ]}
        >
          {selectedRecord && (
            <div>
              <p className='font-medium'>
                Tên: {''}
                <span className='font-bold text-pink-500'>
                  {selectedRecord.fullName}
                </span>
              </p>
              <div className='flex'>
                <p className='font-medium'>
                  Ngày học:{' '}
                  <span className='line-under'>
                    {selectedRecord.date} <br />
                  </span>
                  Thời gian:{' '}
                  <span className='font-bold'>
                    từ {selectedRecord.timeStart} tới {selectedRecord.timeEnd}{' '}
                  </span>
                </p>
              </div>
              <p className='font-medium'>
                Môn học:
                <span className='text-blue-400'> {selectedRecord.subject}</span>
              </p>
              <p className='font-medium'>
                Học phí: {''}
                <span className='text-red-500'>{selectedRecord.price}</span>
              </p>
            </div>
          )}
        </Modal> */}
      </div>
    </>
  )
}
