import { useContext } from 'react'
import { Navigate, Outlet, useRoutes } from 'react-router-dom'
import { path } from './constant/path'
import { AppContext } from './context/app.context'
import MainLayout from './layout/MainLayout'
import RegisterLayout from './layout/RegisterLayout/RegisterLayout'
import AdminLayout from './pages/Admin/AdminLayout'
import Home from './pages/Home'
import Login from './pages/Login'
import ModeratorLayout from './pages/Moderator/ModeratorLayout/ModeratorLayout'
import Register from './pages/Register'
import UserLayout from './pages/User/layout/UserLayout'

import AdminConfirmRegister from './pages/Admin/Components/AdminConfirmRegister/AdminConfirmRegister'
import AdminListTutor from './pages/Admin/Components/AdminListTutor'
import AdminRejectRegister from './pages/Admin/Components/AdminRejectRegister'
import StudentList from './pages/Admin/Components/AdminStudentList'
import SessionList from './pages/Admin/Components/SessionList'
import CheckOut from './pages/CheckOut'
import Deposit from './pages/Deposit'
import ModAccountStudent from './pages/Moderator/Components/ModAccountStudent'
import ModTutorResRegis from './pages/Moderator/Components/ModTutorResRegis/ModTutorResRegis'
import StudentRes from './pages/Moderator/Components/StudentRes'
import PaymentCallback from './pages/PaymentCallBack'
import RegisterAsTuTor from './pages/RegisterAsTutor/RegisterAsTutor'
import RequestList from './pages/RequestList'
import ReStuCurrentPage from './pages/StudentViewRequestList/Layout/ReStuCurrentPage'
import ReStuPending from './pages/StudentViewRequestList/Layout/ReStuPending'

import ChangPassword from './pages/User/pages/ChangePassword'
import Profile from './pages/User/pages/Profile'
// import PaymentSuccess from './pages/PaymentCallBack/PaymentSuccess/PaymentSuccess'
import AdminStudentReq from './pages/Admin/Components/AdminStudentReq'
import AdminStudentReqApproved from './pages/Admin/Components/AdminStudentReqApproved'
import AdminStudentReqRejected from './pages/Admin/Components/AdminStudentReqRejected'
import AdminViewComplaint from './pages/Admin/Components/AdminViewComplaint'
import Dashboard from './pages/Admin/Components/Dashboard'
import TransactionList from './pages/Admin/Components/TransactionList'
import ModViewComplaint from './pages/Moderator/Components/ModViewComplaint'
import MyClass from './pages/MyClass'
import MyClasLayout from './pages/MyClass/MyClassLayout'
import PaymentFail from './pages/PaymentCallBack/PaymentFail'
import PaymentSuccess from './pages/PaymentCallBack/PaymentSuccess/PaymentSuccess'
import BookedService from './pages/Sevice/BookedService'

import SidebarMenu from './components/SidebarMenu'
import ApplicationReject from './pages/ApplicationTutor/ApplicationReject'
import ApplicationSpending from './pages/ApplicationTutor/ApplicationSpending'
import ApplicationSuccess from './pages/ApplicationTutor/ApplicationSuccess'
import MyService from './pages/Sevice/MyService'
import ServiceList from './pages/Sevice/ServiceList'
import TutorViewOwnService from './pages/Sevice/TutorViewOwnService'
import ReStuReject from './pages/StudentViewRequestList/Layout/ReStuReject'
import Support from './pages/Support'
import TutorList from './pages/TutorList'
import ServiceListOfTutor from './pages/TutorList/components/ServiceListOfTutor'
import TutorListInClass from './pages/TutorListInClass'
import ProfileTT from './pages/User/pages/ProfileTT'

function ProtectedRoute() {
  const { isAuthenticated } = useContext(AppContext)
  return isAuthenticated ? <Outlet /> : <Navigate to={path.login} />
}

function RejectedRoute() {
  const { isAuthenticated } = useContext(AppContext)
  return !isAuthenticated ? <Outlet /> : <Navigate to={path.home} />
}

export default function useRouteElements() {
  const routeElements = useRoutes([
    {
      path: '',
      element: <RejectedRoute />,
      children: [
        {
          path: path.register, // user
          element: (
            <RegisterLayout>
              <Register />
            </RegisterLayout>
          )
        },

        {
          path: path.login, // user
          element: (
            <RegisterLayout>
              <Login />
            </RegisterLayout>
          )
        }
      ]
    },

    {
      path: '',
      element: <ProtectedRoute />,
      children: [
        {
          path: path.home,
          element: (
            <MainLayout>
              <Home />
            </MainLayout>
          ),
          index: true
        },
        {
          path: path.registerAsTutor, //student
          element: (
            <MainLayout>
              <RegisterAsTuTor />
            </MainLayout>
          )
        },
        {
          path: path.user, //user-----------------------------------------------------------------------------------
          element: (
            <MainLayout>
              <UserLayout />
            </MainLayout>
          ),
          children: [
            { index: true, element: <Profile /> },
            { path: path.changePassword, element: <ChangPassword /> },
            { path: path.profileTT, element: <ProfileTT /> }
          ]
        },

        {
          path: path.requestList, //tutor and student-------------------------------------------------------------------------
          element: (
            <MainLayout>
              <RequestList />
            </MainLayout>
          )
        },

        {
          path: path.myService, //student
          element: (
            <MainLayout>
              <MyService />
            </MainLayout>
          )
        },
        {
          path: path.tutorviewAllOwnService, //student
          element: (
            <MainLayout>
              <TutorViewOwnService />
            </MainLayout>
          )
        },
        {
          path: path.support, //student
          element: (
            <MainLayout>
              <Support />
            </MainLayout>
          )
        },
        {
          path: path.sideBarMenu, //student
          element: (
            <MainLayout>
              <SidebarMenu />
            </MainLayout>
          )
        },
        {
          path: path.service, //student
          element: (
            <MainLayout>
              <ServiceList />
            </MainLayout>
          )
        },
        {
          path: path.checkOut, //student
          element: (
            <MainLayout>
              <CheckOut />
            </MainLayout>
          )
        },
        {
          path: path.deposit,
          element: (
            <MainLayout>
              <Deposit />
            </MainLayout>
          )
        },
        {
          path: path.tutorList,
          element: (
            <MainLayout>
              <TutorList />
            </MainLayout>
          )
        },
        {
          path: '/tutors/:idReq', //student--------------------------------------------------------------------------
          element: (
            <MainLayout>
              <TutorListInClass />
            </MainLayout>
          )
        },
        {
          path: '/services/:tutorId', //student--------------------------------------------------------------------------
          element: (
            <MainLayout>
              <ServiceListOfTutor />
            </MainLayout>
          )
        },
        {
          path: path.requestStudentCurrent, //student
          element: (
            <MainLayout>
              <ReStuCurrentPage />
            </MainLayout>
          )
        },
        {
          path: path.myService, //student
          element: (
            <MainLayout>
              <MyService />
            </MainLayout>
          )
        },
        {
          path: path.checkOut, // student
          element: (
            <MainLayout>
              <CheckOut />
            </MainLayout>
          )
        },
        {
          path: path.paymentcallback, // student // này là hiển thị thông báo nạp tiền thành công nha fen
          element: (
            <MainLayout>
              <PaymentCallback />
            </MainLayout>
          )
        },
        {
          path: path.paymentSucsess, // student // này là hiển thị thông báo nạp tiền thành công nha fen
          element: (
            <MainLayout>
              <PaymentSuccess />
            </MainLayout>
          )
        },
        {
          path: path.paymentFail, // student // này là hiển thị thông báo nạp tiền thất bại nha fen
          element: (
            <MainLayout>
              <PaymentFail />
            </MainLayout>
          )
        },
        {
          path: path.studentViewRequestList, //student
          element: (
            <MainLayout>
              <ReStuPending />
            </MainLayout>
          )
        },
        {
          path: path.requestStudentReject, //student
          element: (
            <MainLayout>
              <ReStuReject />
            </MainLayout>
          )
        },

        {
          path: path.bookedService, //student
          element: (
            <MainLayout>
              <BookedService />
            </MainLayout>
          )
        },
        {
          path: path.myClass, //student
          element: (
            <MainLayout>
              <MyClass />
            </MainLayout>
          )
        },

        {
          path: path.myClassLayout, //student
          element: (
            <MainLayout>
              <MyClasLayout />
            </MainLayout>
          )
        },
        {
          path: path.Moderator.mod, //mod-------------------------------------------------------------------------------
          element: <ModeratorLayout />,
          children: [
            { index: true, element: <StudentRes /> },
            {
              path: path.Moderator.tutorResRegis,
              element: <ModTutorResRegis />
            },
            {
              path: path.Moderator.listAccountStudent,
              element: <ModAccountStudent />
            },
            {
              path: path.Moderator.listComplant,
              element: <ModViewComplaint />
            }
          ]
        },
        {
          path: path.Admin.admin, //admin--------------------------------------------------------
          element: <AdminLayout />,
          children: [
            {
              index: true,
              element: <Dashboard />
            },
            { path: path.Admin.sessionList, element: <SessionList /> },
            // { path: path.Admin.viewAllComplaint, element: < /> },
            { path: path.Admin.studentlist, element: <StudentList /> },
            { path: path.Admin.adminStudentReq, element: <AdminStudentReq /> },
            { path: path.Admin.TransactionList, element: <TransactionList /> },
            {
              path: path.Admin.adminStudentReqApproved,
              element: <AdminStudentReqApproved />
            },
            {
              path: path.Admin.adminStudentReqRejected,
              element: <AdminStudentReqRejected />
            },
            { path: path.Admin.tutorList, element: <AdminListTutor /> },
            {
              path: path.Admin.confirmProfileRegisterTT,
              element: <AdminConfirmRegister />
            },
            {
              path: path.Admin.rejectProfileRegisterTT,
              element: <AdminRejectRegister />
            },
            {
              path: path.Admin.listComplant,
              element: <AdminViewComplaint />
            }
          ]
        },

        {
          path: path.deposit,
          element: (
            <MainLayout>
              <Deposit />
            </MainLayout>
          )
        },
        // {
        //   path: path.applicationTutor, //student
        //   element: (
        //     <MainLayout>
        //       <MainLayoutApplicationTutor />
        //     </MainLayout>
        //   )
        // },
        {
          path: path.tutorViewApplicationReject, //student
          element: (
            <MainLayout>
              <ApplicationReject />
            </MainLayout>
          )
        },
        {
          path: path.tutorViewApplicationSpending, //student
          element: (
            <MainLayout>
              <ApplicationSpending />
            </MainLayout>
          )
        },
        {
          path: path.tutorViewApplication, //student
          element: (
            <MainLayout>
              <ApplicationSuccess />
            </MainLayout>
          )
        }
      ]
    }
  ])

  return routeElements
}
