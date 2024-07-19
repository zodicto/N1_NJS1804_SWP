import { useState } from 'react'
import { Button, Modal, Table, TableColumnsType } from 'antd'
import Search from 'antd/es/transfer/search'
import StudentMenu from '../AdminMenu/StudentMenu/StudentMenu'
import { adminAPI } from '../../../../api/admin.api'
import { useMutation, useQuery } from '@tanstack/react-query'
import { toast } from 'react-toastify'

const onChange = () => {}
export default function AdminStudentList() {
  // Lấy danh sách học sinh
  const { data: StudentData, refetch } = useQuery<DataTypeStu[]>({
    queryKey: ['StudentList'],
    queryFn: () => adminAPI.getStudentList() // đổi lại thành api cảu admin nha, vẫn đang xài api của mod nhé
  })
  //const [searchText, setSearchText] = useState('');// liên quan đến giá trị input vào search
  const [selectedRecord, setSelectedRecord] = useState<DataTypeStu | null>(null)
  const [visible, setVisible] = useState(false)

  const showDetail = (record: DataTypeStu) => {
    setSelectedRecord(record)
    setVisible(true)
  }
  const handleCancel = () => {
    setVisible(false)
    setSelectedRecord(null)
  }
  const removeMutation = useMutation({
    mutationFn: (idAccount: string) => adminAPI.deleteAccount(idAccount),
    onSuccess: () => {
      toast.success('Đã xóa tài khoản')
      refetch() // Gọi lại API để cập nhật lại danh sách yêu cầu
      setVisible(false)
    }
  })
  const handleDelete = () => {
    if (selectedRecord) {
      removeMutation.mutate(selectedRecord.id)
      console.log('id của thằng request nè ', selectedRecord.id)
    }
  }
  const columns: TableColumnsType<DataTypeStu> = [
    {
      // định nghĩa từng cột
      title: 'Tên học sinh', // tên của cột hay còn gọi là header của cột
      dataIndex: 'fullName', // xác định trường nào trong interface DataType
      defaultSortOrder: 'descend',
      onFilter: (value, record) =>
        record.fullName.indexOf(value as string) === 0,
      sorter: (a, b) => a.fullName.length - b.fullName.length,
      width: 200,
      fixed: 'left'
    },
    {
      title: 'Ngày sinh',
      dataIndex: 'date_of_birth',
      defaultSortOrder: 'descend',
      width: 200,
      sorter: (a, b) =>
        new Date(a.date_of_birth).getTime() -
        new Date(b.date_of_birth).getTime()
    },
    {
      title: 'Giới Tính',
      dataIndex: 'gender',
      sorter: (a, b) => parseInt(a.gender) - parseInt(b.gender),
      width: 200
    },
    {
      title: 'Số điện thoại',
      dataIndex: 'phone',
      //sorter: (a, b) => parseInt(a.gender) - parseInt(b.gender),
      width: 200
    },
    {
      title: 'Email',
      dataIndex: 'email',
      //sorter: (a, b) => parseInt(a.gender) - parseInt(b.gender),
      width: 200
    },
    {
      title: 'Detail',
      dataIndex: 'detail',
      className: 'TextAlign',
      width: 100,
      render: (text: string, record: DataTypeStu) => (
        <div className='flex gap-1'>
          <button
            className='p-1 border border-red-500 rounded-lg hover:bg-red-500 active:bg-red-700'
            onClick={() => showDetail(record)}
          >
            Chi tiết
          </button>
        </div>
      ),
      fixed: 'right'
    }
  ]
  return (
    <>
      <div>
        <div className='text-left '>Quản lí học sinh</div>
        <StudentMenu list='list' req='' app='' rej='' />
        <div className='text-left shadow-sm shadow-black border-4 pt-5 h-[629px] rounded-t-xl mt-6'>
          <div className='mb-5'>
            <Search
            // inputText={searchText}
            // placeHolder="Search đi nè"
            // setInputValue={setSearchText}
            // label="Q"
            />
          </div>
          <Table
            className=''
            columns={columns}
            dataSource={StudentData}
            pagination={{ pageSize: 6 }}
            onChange={onChange}
            showSorterTooltip={{ target: 'sorter-icon' }}
            scroll={{ x: 1300, y: 400 }}
          />
        </div>
        <div>
          <Modal
            title='Chi tiết'
            visible={visible}
            onCancel={handleCancel}
            footer={[
              <Button key='back' onClick={handleCancel}>
                Sửa(chưa thêm api)
              </Button>,
              <Button key='back' onClick={handleDelete}>
                Xóa
              </Button>
            ]}
          >
            {selectedRecord && (
              <div>
                <p>
                  {' '}
                  Tên :{' '}
                  {selectedRecord.fullName
                    ? selectedRecord.fullName
                    : 'Chưa cập nhập'}
                </p>
                <p>
                  {' '}
                  ID: {selectedRecord.id ? selectedRecord.id : 'Chưa cập nhập'}
                </p>
                <p>
                  {' '}
                  Ngày sinh :{' '}
                  {selectedRecord.date_of_birth
                    ? selectedRecord.date_of_birth
                    : 'Chưa cập nhập'}
                </p>
                <p>
                  {' '}
                  Giới tính :{' '}
                  {selectedRecord.gender
                    ? selectedRecord.gender
                    : 'Chưa cập nhập'}
                </p>
                <p>
                  {' '}
                  Email :{' '}
                  {selectedRecord.email
                    ? selectedRecord.email
                    : 'Chưa cập nhập'}
                </p>
                <p>
                  {' '}
                  Số điện thoại :{' '}
                  {selectedRecord.phone
                    ? selectedRecord.phone
                    : 'Chưa cập nhập'}
                </p>
              </div>
            )}
          </Modal>
        </div>
      </div>
    </>
  )
}
