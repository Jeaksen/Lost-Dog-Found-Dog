import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity ,ScrollView,SafeAreaView,FlatList} from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import CommentListItem from './Helpers/CommentListItem';
const {width, height} = Dimensions.get("screen")


export default class CommentListPage extends React.Component {
  state={
    info: " normal ",
    CommentList: [],
  }

  getCommentList = ()=>{
      const data={
        dogId: this.props.item.dogId
      }
    this.props.Navi.RunOnBackend("getComment",data).then((responseData)=>{
      //console.log(responseData)
      this.setState({CommentList: responseData.comments});
      console.log("succes list of comments is ready")
      console.log(this.state.CommentList[0].id)
    }).catch((x)=>
        console.log("Comments Error" + (x))
      )
  }

  constructor(props) {
    super(props);
    this.getCommentList();
   }

  render(){
    return(
        <View style={styles.content}>
            {
                this.state.CommentList.map((e, index)=><CommentListItem key={index} id={e.id} text={e.text} ></CommentListItem>)
            }
        </View>
  )
  }
}

const styles = StyleSheet.create({
  inputtext: {
    fontSize: 16,
    height: 30,
    width: width*0.5,
    borderColor: '#000000',
    borderWidth: 1,
    borderRadius: 5,
    paddingLeft: 5,
    marginVertical: 10,
  },
  content: {
    marginTop:30,
    margin: 15,
    height: '90%',
    alignSelf: 'center',
    justifyContent: 'center',
  },
  loginButton:{
    marginTop: 20,
    backgroundColor: 'black',
    width: width*0.2,
    height: height*0.05,
    marginLeft: 'auto',
    marginRight: 'auto',
},
logintext:{
    marginTop: 'auto',
    marginBottom: 'auto',
    fontSize: 15,
    color: 'white',
    textAlign: 'center',
},
Title:{
  marginBottom: 50,
  fontSize: 20,
  textAlign: 'center',
  fontWeight: 'bold',
},
});
